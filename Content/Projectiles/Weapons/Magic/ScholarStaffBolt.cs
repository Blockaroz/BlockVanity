using System;
using System.Collections.Generic;
using BlockVanity.Common.Graphics;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Projectiles.Weapons.Magic;

public class ScholarStaffBolt : ModProjectile
{
    private static SoundStyle HitSound;

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
        //HitSound = new SoundStyle();
    }

    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.timeLeft = 600;
    }

    public ref float Speed => ref Projectile.ai[0];

    public override void AI()
    {
        Projectile.velocity *= 0.9f;

        float speed = 12f * Speed;

        Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity.SafeNormalize(Vector2.Zero) * speed, 0.16f);

        int target = Projectile.FindTargetWithLineOfSight(600);
        if (target > -1 && Projectile.timeLeft < 590)
        {
            Projectile.localAI[0] += 0.5f;
            speed = 16f * Speed;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, new Vector2(speed, 0).RotatedBy(Projectile.AngleTo(Main.npc[target].Center)), 0.066f);
        }

        Projectile.rotation = Projectile.velocity.ToRotation();

        Projectile.localAI[0] += 0.5f + Speed * 0.5f;

        if (Projectile.timeLeft % 18 == 0 || Main.rand.NextBool(25))
            ParticleEngine.particles.NewParticle(new MagicTrailParticle(EnergyColor with { A = 0 }, true, 10), Projectile.Center + Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(10, 10), Projectile.velocity * 0.5f, 0f, Main.rand.NextFloat(0.5f, 1f));

        Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.33f);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) => true;

    public override void OnKill(int timeLeft)
    {
        SoundStyle hitSound = SoundID.DD2_LightningBugZap;
        hitSound.MaxInstances = 0;
        hitSound.PitchVariance = 0.1f;
        SoundEngine.PlaySound(hitSound, Projectile.Center);

        for (int i = 0; i < 16; i++)
        {
            Vector2 offset = Main.rand.NextVector2Circular(24, 24);
            ParticleEngine.particles.NewParticle(new MagicTrailParticle(EnergyColor with { A = 0 }, true), Projectile.Center + offset / 2f, offset * Main.rand.NextFloat(0.3f) * (i / 16f) + Projectile.velocity * 0.1f, 0f, Main.rand.NextFloat(1f, 2f));
        }

        ParticleEngine.particles.NewParticle(new ScholarStaffExplosionParticle(EnergyColor with { A = 0 }, true), Projectile.Center, Vector2.Zero, 0f, Projectile.scale);
    }

    public static readonly Color EnergyColor = Color.Lerp(Color.DodgerBlue, Color.Turquoise, 0.6f);

    private static RenderTargetDrawContent boltContent;
    private VertexStrip _verticalStrip;
    private VertexStrip _horizontalStrip;
    public static readonly int PixelSize = 2;

    public override void Load()
    {
        Main.ContentThatNeedsRenderTargets.Add(boltContent = new RenderTargetDrawContent(Main.projectile.Length));
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Texture2D glow = AllAssets.Textures.Glow[0].Value;

        Color color = EnergyColor with { A = 50 };

        boltContent.Request(200, 200, Projectile.whoAmI, spriteBatch =>
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            Vector2 projCenter = new Vector2(100);

            List<Vector2> strip0 = new List<Vector2>();
            List<Vector2> strip1 = new List<Vector2>();
            List<float> rot0 = new List<float>();
            List<float> rot1 = new List<float>();

            float stripLength = 48;
            float t = Projectile.localAI[0] * 2f * (Projectile.velocity.X > 0 ? 1 : -1);
            for (int i = 0; i < (int)stripLength; i++)
            {
                float rad = (16f + (float)Math.Sin(t * 0.15f % MathHelper.TwoPi) * 2) / PixelSize;
                Vector2 offRot0 = new Vector2(rad * (float)Math.Sqrt(Utils.GetLerpValue(stripLength, stripLength * 0.5f, i, true)), 0).RotatedBy(t * 0.1f + i / stripLength * 3f);
                Vector2 offset0 = new Vector2(offRot0.X, offRot0.Y * 0.4f).RotatedBy(Projectile.rotation + t * 0.05f);
                strip0.Add(offset0);
                rot0.Add(offset0.ToRotation() - MathHelper.PiOver2);
                Vector2 offRot1 = new Vector2(rad, 0).RotatedBy(t * 0.15f + i / stripLength * 5f + MathHelper.Pi / 2f);
                Vector2 offset1 = new Vector2(offRot1.X * 0.6f, -offRot1.Y).RotatedBy(Projectile.rotation + t * 0.05f + MathHelper.TwoPi);
                strip1.Add(offset1);
                rot1.Add(offset1.ToRotation() - MathHelper.PiOver2);
            }

            Effect shader = AllAssets.Effects.BasicTrail.Value;

            shader.Parameters["transformMatrix"].SetValue(VanityUtils.NormalizedEffectMatrix);
            shader.Parameters["uTexture0"].SetValue(TextureAssets.Extra[197].Value);
            shader.Parameters["uTexture1"].SetValue(TextureAssets.Extra[194].Value);
            shader.Parameters["uColor"].SetValue((color with { A = 70 }).ToVector4() * 1.5f);
            shader.Parameters["uBlackAlpha"].SetValue(1f);
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.1f % 1f);

            shader.CurrentTechnique.Passes[0].Apply();

            _horizontalStrip ??= new VertexStrip();
            _verticalStrip ??= new VertexStrip();
            _horizontalStrip.PrepareStrip(strip0.ToArray(), rot0.ToArray(), p => Color.White with { A = 100 }, p => Utils.GetLerpValue(1f, 0.6f, p, true) * Utils.GetLerpValue(0, 0.5f, p, true) * 24f / PixelSize, projCenter, strip0.Count, true);
            _verticalStrip.PrepareStrip(strip1.ToArray(), rot1.ToArray(), p => Color.White with { A = 100 }, p => Utils.GetLerpValue(1f, 0.6f, p, true) * Utils.GetLerpValue(0, 0.5f, p, true) * 24f / PixelSize, projCenter, strip1.Count, true);
            _horizontalStrip.DrawTrail();
            _verticalStrip.DrawTrail();

            //Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            Main.EntitySpriteDraw(texture, projCenter, texture.Frame(), color, Projectile.rotation + MathHelper.PiOver2, texture.Size() * new Vector2(0.5f, 0.4f), new Vector2(1.1f, 1.2f) / PixelSize, 0, 0);
            Main.EntitySpriteDraw(texture, projCenter, texture.Frame(), new Color(225, 255, 255, 0), Projectile.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(0.7f, 0.9f) / PixelSize, 0, 0);

            spriteBatch.End();
        });

        if (boltContent.IsTargetReady(Projectile.whoAmI))
        {
            Texture2D boltTexture = boltContent.GetTarget(Projectile.whoAmI);
            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), Color.Black * 0.8f, 0f, glow.Size() * 0.5f, 0.4f * Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(boltTexture, Projectile.Center - Main.screenPosition, boltTexture.Frame(), Color.White with { A = 60 }, 0, boltTexture.Size() * 0.5f, PixelSize * Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), color with { A = 0 }, 0f, glow.Size() * 0.5f, 0.35f * Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), color with { A = 0 } * 0.3f, 0f, glow.Size() * 0.5f, 0.6f * Projectile.scale, 0, 0);
        }

        return false;
    }
}
