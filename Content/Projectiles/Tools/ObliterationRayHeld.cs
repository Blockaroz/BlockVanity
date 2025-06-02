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
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.ownerHitCheck = true;
        Projectile.aiStyle = -1;
        Projectile.hide = true;
        Projectile.manualDirectionChange = true;
    }

    public ref float Time => ref Projectile.ai[0];
    public ref float UseState => ref Projectile.ai[1];

    public ref Player Player => ref Main.player[Projectile.owner];

    public Vector2 tileTarget;
    public Vector2 visualTarget;

    public bool Digging { get => UseState == 1; set => UseState = 1; }
    public bool Hurting { get => UseState == 2; set => UseState = 2; }

    public bool Mineable => Projectile.CanExplodeTile((int)tileTarget.X, (int)tileTarget.Y)
        && WorldGen.SolidTile((int)tileTarget.X, (int)tileTarget.Y)
        && Player.IsTargetTileInItemRange(Player.HeldItem)
        && !Hurting;

    public override void AI()
    {
        Projectile.timeLeft = 60;
        UseState = 0;

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
            }
            else
                Projectile.Kill();
        }

        Vector2 npcCenter = tileTarget.ToWorldCoordinates();
        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (npc.CanBeChasedBy(Player) &&
                npc.Distance(tileTarget.ToWorldCoordinates()) < 120 && 
                npc.Distance(Player.Center) < 600)
            {
                Hurting = true;
                npcCenter = npc.Center;
                break;
            }    
        }

        if (Hurting)
        {
            Player.toolTime = 4;
            Player.noBuilding = true;

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(npcCenter).SafeNormalize(Vector2.Zero), 0.9f);
            tileTarget = npcCenter.ToTileCoordinates().ToVector2();
            visualTarget = Vector2.Lerp(visualTarget, npcCenter, 0.9f);
        }
        else if (Mineable)
            Digging = true;

        if (Time <= 0)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            visualTarget = GetVisualTarget();
        }

        Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
        Player.ChangeDir(Projectile.direction);
        Player.heldProj = Projectile.whoAmI;
        Player.SetDummyItemTime(5);
        Projectile.Center = Player.RotatedRelativePoint(Player.HandPosition.Value - new Vector2(0, Player.gfxOffY)) + Main.rand.NextVector2Circular(1, 1);
        Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(tileTarget.ToWorldCoordinates()), 0.25f);
        Player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

        if (++Projectile.frame > 5)
            Projectile.frame = 0;

        visualTarget = Vector2.Lerp(visualTarget, GetVisualTarget(), 0.5f);

        if (Main.myPlayer == Projectile.owner)
        {
            if (Player.toolTime == 0 && Mineable)
            {
                Player.PickTile((int)(tileTarget.X), (int)(tileTarget.Y), Player.HeldItem.pick);

                int offX = Main.rand.Next(-1, 2);
                int offY = Main.rand.Next(-1, 2);
                if (Main.rand.NextBool(3))
                {
                    if (WorldGen.InWorld((int)(tileTarget.X + offX), (int)(tileTarget.Y + offY)))
                        Player.PickTile((int)(tileTarget.X + offX), (int)(tileTarget.Y + offY), Player.HeldItem.pick);
                }
            }
        }

        MakeLightningPoints();

        Vector2 gunEnd = Projectile.Center + new Vector2(30, -2 * Projectile.direction).RotatedBy(Projectile.rotation);
        Lighting.AddLight(gunEnd, Color.Orange.ToVector3() * 0.5f);

        if (Digging || Hurting)
        {
            Lighting.AddLight(visualTarget, Color.Lerp(Color.Orange, Color.Gold, Main.rand.NextFloat()).ToVector3() * 0.5f);

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
            //SoundEngine.PlaySound(SoundID.Item132, Projectile.Center);
        }

        Time++;

        Projectile.scale = 0.8f;
    }

    public Vector2 GetVisualTarget()
    {
        if (Hurting && Time > 0)
            return visualTarget;

        if (Digging)
            return tileTarget.ToWorldCoordinates();

        return (Projectile.Center + new Vector2(130f, 0).RotatedBy(Projectile.rotation)).ToTileCoordinates().ToWorldCoordinates();
    }

    public override void ModifyDamageHitbox(ref Rectangle hitbox)
    {
        hitbox.Location = (visualTarget - hitbox.Size() / 2).ToPoint();
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (Hurting)
            return targetHitbox.Intersects(projHitbox);

        return false;
    }

    private ObliterationLightningData _lightning;
    private ObliterationLightningData _lightningThin;

    public void MakeLightningPoints()
    {
        bool active = Digging || Hurting;
        float distance = active ? Projectile.Distance(tileTarget.ToWorldCoordinates()) : 200f;
        Vector2 gunEnd = Projectile.Center + new Vector2(30, -2 * Projectile.direction * Player.gravDir).RotatedBy(Projectile.rotation);
        Vector2 middle = Projectile.Center + new Vector2(distance / 2, 0).RotatedBy(Projectile.rotation);

        int pointCount = Math.Max((int)(distance / 30), 3);
        if (Time == 0)
        {
            _lightning = new ObliterationLightningData(pointCount, -1);
            _lightningThin = new ObliterationLightningData(pointCount, -3);
        }

        _lightning.SetVariables(pointCount, 20f, active);
        _lightning.SetEnds(gunEnd, middle, visualTarget);
        _lightning.Update();
        _lightningThin.SetVariables(pointCount, 15f, active);
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
        Vector2 origin = new Vector2(frame.Width / 2 - 14, frame.Height / 2 + 2 * Projectile.direction * Player.gravDir);

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

        Color glowColor = Color.LightGoldenrodYellow with { A = 0 };
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, glowFrame, glowColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

        return false;
    } 

    public void DrawLightning()
    {
        Color GetLightningColor(float p) => Digging || Hurting ? Color.Lerp(Color.Gold, Color.Orange, p) with { A = 120 } : Color.Lerp(Color.Orange, Color.OrangeRed, p * 0.5f + 0.3f) with { A = 20 } * (1f - p) * 0.66f;
        float GetThinWidth(float p) => (10f + MathF.Sin(p * 8 - Time * 0.1f) * 5f) * Utils.GetLerpValue(1.1f, 0.7f, p, true) * Utils.GetLerpValue(-0.02f, 0.05f, p, true);

        Effect basicTrail = AllAssets.Effects.ObliterationRayBeam.Value;
        basicTrail.Parameters["uTexture0"].SetValue(TextureAssets.Extra[197].Value);
        basicTrail.Parameters["uTexture1"].SetValue(TextureAssets.Extra[197].Value);
        basicTrail.Parameters["uGlowColor"].SetValue(Digging || Hurting ? new Vector4(0.5f, 0.15f, 0f, 0.01f) : Vector4.Zero);
        basicTrail.Parameters["uTime"].SetValue(Time * 0.01f);
        basicTrail.Parameters["transformMatrix"].SetValue(Matrix.CreateScale(0.5f) * VanityUtils.NormalizedTranslationMatrix);
        basicTrail.CurrentTechnique.Passes[0].Apply();

        float[] rotations = Enumerable.Repeat(Projectile.rotation, _lightning.Length).ToArray();

        PrimitiveRenderer.DrawStrip(_lightningThin.Points, rotations, GetLightningColor, GetThinWidth, -Main.screenPosition);

        if (Digging || Hurting) 
        {
            float GetThickWidth(float p) => 25f + MathF.Sin(p * 9f + Time) * 12f;

            PrimitiveRenderer.DrawStrip(_lightning.Points, rotations, GetLightningColor, GetThickWidth, -Main.screenPosition);
        }

        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    public void DrawPixelated(SpriteBatch spriteBatch)
    {
        DrawLightning();
    }
}
