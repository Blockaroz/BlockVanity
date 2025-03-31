using BlockVanity.Common.Graphics;
using BlockVanity.Content.Particles;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Projectiles.Tools;

public class ObliterationRayHeld : ModProjectile, IDrawPixelated
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.ownerHitCheck = true;
        Projectile.aiStyle = -1;
        Projectile.hide = true;
        Projectile.manualDirectionChange = true;
    }

    public ref float Time => ref Projectile.ai[0];

    public ref Player Player => ref Main.player[Projectile.owner];

    public Vector2 tileTarget;
    public Vector2 visualTarget;
    public bool Digging => Projectile.localAI[0] > 0;
    public bool Mineable => Projectile.CanExplodeTile((int)tileTarget.X, (int)tileTarget.Y)
        && WorldGen.SolidTile((int)tileTarget.X, (int)tileTarget.Y)
        && Player.IsTargetTileInItemRange(Player.HeldItem);

    public override void AI()
    {
        Projectile.timeLeft = 60;

        if (Main.myPlayer == Projectile.owner)
        {
            if (Player.channel)
            {
                float holdoutDistance = Player.HeldItem.shootSpeed * Projectile.scale;
                Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - Player.RotatedRelativePoint(Player.MountedCenter));

                if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;

                if (tileTarget.X != Player.tileTargetX || tileTarget.Y != Player.tileTargetY)
                    Projectile.netUpdate = true;

                tileTarget.X = Player.tileTargetX;
                tileTarget.Y = Player.tileTargetY;
                Projectile.velocity = holdoutOffset;
                if (Player.toolTime != 0 && Mineable)
                {
                    int offX = Main.rand.Next(-1, 2);
                    int offY = Main.rand.Next(-1, 2);
                    if (Main.rand.NextBool(3))
                    {
                        if (WorldGen.InWorld((int)(tileTarget.X + offX), (int)(tileTarget.Y + offY)))
                            Player.PickTile((int)(tileTarget.X + offX), (int)(tileTarget.Y + offY), Player.HeldItem.pick);
                    }
                }
            }
            else
                Projectile.Kill();
        }

        if (Mineable)
            Projectile.localAI[0] = 2;
        else if (Player.toolTime != 0)
            Projectile.localAI[0] = 0;

        float distance = Projectile.Distance(tileTarget.ToWorldCoordinates());

        if (Time <= 0)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            visualTarget = GetVisualTarget();
        }

        Vector2 tileTargetInWorld = tileTarget.ToWorldCoordinates();

        Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
        Player.ChangeDir(Projectile.direction);
        Player.heldProj = Projectile.whoAmI;
        Player.SetDummyItemTime(5);
        Projectile.Center = Player.RotatedRelativePoint(Player.HandPosition.Value - new Vector2(0, Player.gfxOffY)) + Main.rand.NextVector2Circular(1, 1);
        Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(tileTargetInWorld), 0.1f);
        Player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

        if (++Projectile.frame > 5)
            Projectile.frame = 0;

        visualTarget = Vector2.Lerp(visualTarget, GetVisualTarget(), 0.5f);

        MakeLightningPoints();

        Vector2 gunEnd = Projectile.Center + new Vector2(30, -2 * Projectile.direction).RotatedBy(Projectile.rotation);
        Lighting.AddLight(gunEnd, Color.Orange.ToVector3() * 0.5f);

        if (Mineable)
        {
            Lighting.AddLight(tileTargetInWorld, Color.Lerp(Color.Orange, Color.Gold, Main.rand.NextFloat()).ToVector3() * 0.5f);
            DelegateMethods.v3_1 = Color.Orange.ToVector3() * 0.5f;
            Utils.PlotTileLine(Projectile.Center, visualTarget, 1f, DelegateMethods.CastLightOpen);

            for (int i = 0; i < Main.rand.Next(1, 4); i++)
            {
                float light = Main.rand.NextFloat();
                Color lightColor = Color.Lerp(Color.Gold, Color.LightGoldenrodYellow, light);
                Color darkColor = light > 0.3f ? Color.Gold : Color.OrangeRed;
                PixelSpark particle = PixelSpark.pool.RequestParticle();
                particle.Prepare(visualTarget, Main.rand.NextVector2Circular(5, 5), Vector2.UnitY * 0.33f, lightColor with { A = 20 }, darkColor with { A = 20 } * 0.4f, 0.2f + Main.rand.NextFloat(0.4f), !Main.rand.NextBool(8));
                ParticleEngine.Particles.Add(particle);
            }

            if (Main.rand.NextBool())
            {
                float light = Main.rand.NextFloat();
                Color lightColor = Color.Lerp(Color.Gold, Color.LightGoldenrodYellow, light);
                Color darkColor = light > 0.3f ? Color.Gold : Color.OrangeRed;
                PixelSpark particle = PixelSpark.pool.RequestParticle();
                particle.Prepare(gunEnd, Main.rand.NextVector2Circular(4, 4) + Projectile.velocity * 0.2f, Vector2.UnitY * 0.33f, lightColor with { A = 20 }, darkColor with { A = 20 } * 0.4f, 0.1f, !Main.rand.NextBool(8));
                ParticleEngine.Particles.Add(particle);
            }
        }

        if (Time % 20 == 0)
        {
            SoundEngine.PlaySound(SoundID.Item132, Projectile.Center);
        }

        Time++;
    }

    public Vector2 GetVisualTarget()
    {
        if (Mineable)
            return tileTarget.ToWorldCoordinates();

        return (Projectile.Center + new Vector2(130f, 0).RotatedBy(Projectile.rotation)).ToTileCoordinates().ToWorldCoordinates();
    }

    private ObliterationLightningData _lightning;
    private ObliterationLightningData _lightningThin;

    public void MakeLightningPoints()
    {
        float distance = Digging ? Projectile.Distance(tileTarget.ToWorldCoordinates()) : 200f;
        Vector2 gunEnd = Projectile.Center + new Vector2(30, -2 * Projectile.direction * Player.gravDir).RotatedBy(Projectile.rotation);
        Vector2 middle = Projectile.Center + new Vector2(distance / 2, 0).RotatedBy(Projectile.rotation);

        int pointCount = Math.Max((int)(distance / 20), 1);
        if (Time == 0)
        {
            _lightning = new ObliterationLightningData(pointCount);
            _lightningThin = new ObliterationLightningData(pointCount);
        }

        _lightning.SetVariables(pointCount, 50f, Digging);
        _lightning.SetEnds(gunEnd, middle, visualTarget);
        _lightning.Update();
        _lightningThin.SetVariables(pointCount, 50f, Digging);
        _lightningThin.SetEnds(gunEnd, middle, visualTarget);
        _lightningThin.Update();
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.WriteVector2(tileTarget);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        tileTarget = reader.ReadVector2();
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        PixelatedLayers.OverPlayers.Add(this);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Rectangle frame = texture.Frame(2, 6, 0, Projectile.frame);
        Rectangle glowFrame = texture.Frame(2, 6, 1, Projectile.frame);

        SpriteEffects spriteEffects = Projectile.direction * Player.gravDir < 0 ? SpriteEffects.FlipVertically : 0;
        Vector2 origin = new Vector2(frame.Width / 2 - 8, frame.Height / 2 + 2 * Projectile.direction * Player.gravDir);

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

        Color glowColor = Color.LightGoldenrodYellow with { A = 0 };
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, glowFrame, glowColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

        return false;
    } 

    public void DrawLightning()
    {
        Color GetLightningColor(float p) => Digging ? Color.Lerp(Color.Gold, Color.Orange, p) with { A = 30 } : Color.Lerp(Color.Orange, Color.OrangeRed, p * 0.5f + 0.3f) with { A = 20 } * (1f - p) * 0.66f;
        float GetThinWidth(float p) => (12f + MathF.Sin(p * 10f + Time) * 5f);

        Effect basicTrail = AllAssets.Effects.ObliterationRayBeam.Value;
        basicTrail.Parameters["uTexture0"].SetValue(TextureAssets.Extra[197].Value);
        basicTrail.Parameters["uTexture1"].SetValue(TextureAssets.Extra[197].Value);
        basicTrail.Parameters["uGlowColor"].SetValue(Digging ? new Vector4(0.5f, 0.15f, 0f, 0.01f) : Vector4.Zero);
        basicTrail.Parameters["uTime"].SetValue(Time * 0.1f);
        basicTrail.Parameters["transformMatrix"].SetValue(VanityUtils.NormalizedMatrixForPixelization);
        basicTrail.CurrentTechnique.Passes[0].Apply();

        Vector2[] points = _lightningThin.GetPoints();
        float[] rotations = Enumerable.Repeat(Projectile.rotation, points.Length).ToArray();

        PrimitiveRenderer.DrawStrip(points, rotations, GetLightningColor, GetThinWidth, -Main.screenPosition);

        if (Digging)
        {
            points = _lightning.GetPoints();
            float GetThickWidth(float p) => 18f + MathF.Sin(p * 9f + Time) * 5f;

            PrimitiveRenderer.DrawStrip(points, rotations, GetLightningColor, GetThickWidth, -Main.screenPosition);
        }

        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    public void DrawPixelated(SpriteBatch spriteBatch)
    {
        DrawLightning();
    }
}
