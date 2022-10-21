using Terraria.ID;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ParticleEngine;
using BlockVanity.Content.Particles;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.Graphics;
using BlockVanity.Common.Metaballs;
using System.Linq;
using log4net.Util;

namespace BlockVanity.Content.Projectiles.Weapons.Magic
{
    public class ScholarStaffBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
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
            Projectile.manualDirectionChange = true;
        }

        public override void AI()
        {
            Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.localAI[0]++;

            float speed = 12f;
            int target = Projectile.FindTargetWithLineOfSight(600);
            if (target > -1 && Projectile.timeLeft < 590)
            {
                Projectile.localAI[0] += 0.5f;
                speed = 16f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, new Vector2(speed, 0).RotatedBy(Projectile.AngleTo(Main.npc[target].Center)), 0.06f);
            }
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity.SafeNormalize(Vector2.Zero) * speed, 0.2f);

            if (Main.rand.NextBool(14))
            {
                Color particleColor = Color.Lerp(Color.MediumTurquoise, Color.LightCyan, Main.rand.NextFloat(0.5f));
                particleColor.A = 0;
                Vector2 particleVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedByRandom(0.3f) * Main.rand.NextFloat(2f);
                Particle pixel = Particle.NewParticle(Particle.ParticleType<MagicPixelParticle>(), Projectile.Center + Main.rand.NextVector2Circular(15, 15), particleVelocity, particleColor, 1f);
                pixel.emit = true;
            }

            Projectile.scale = (float)Math.Sqrt(Utils.GetLerpValue(598, 585, Projectile.timeLeft, true));

            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.3f);
        }

        public override void Kill(int timeLeft)
        {
            SoundStyle hitSound = SoundID.DD2_GoblinBomb;
            hitSound.MaxInstances = 0;
            hitSound.PitchVariance = 0.1f;
            SoundEngine.PlaySound(hitSound, Projectile.Center);

            for (int i = 0; i < Main.rand.Next(25, 30); i++)
            {
                Color particleColor = Color.Lerp(Color.MediumTurquoise, Color.LightCyan, Main.rand.NextFloat(0.5f));
                particleColor.A = 0;
                Particle pixel = Particle.NewParticle(Particle.ParticleType<MagicPixelParticle>(), Projectile.Center + Main.rand.NextVector2CircularEdge(8, 8) * Projectile.scale, Main.rand.NextVector2Circular(3, 3), particleColor, 1f + Main.rand.NextFloat(2f));
                pixel.emit = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Content/Projectiles/Weapons/Magic/ScholarStaffBolt");
            Asset<Texture2D> glow = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/SoftGlow");
            float ballScale = Projectile.scale * (0.7f + (float)Math.Cos(Projectile.localAI[0] * 0.15f % MathHelper.TwoPi) * 0.12f);

            Color glowColor = Color.MediumTurquoise;
            glowColor.A = 0;

            Main.EntitySpriteDraw(texture.Value, (Projectile.Center - Main.screenPosition) / 1f, null, glowColor, 0, texture.Size() * 0.5f, ballScale * 0.8f, 0, 0);
            Main.EntitySpriteDraw(texture.Value, (Projectile.Center - Main.screenPosition) / 1f, null, new Color(255, 255, 255, 0), 0, texture.Size() * 0.5f, ballScale * 0.56f, 0, 0);
            Main.EntitySpriteDraw(glow.Value, (Projectile.Center - Main.screenPosition) / 1f, null, glowColor * 0.3f, 0, glow.Size() * 0.5f, ballScale * 0.66f, 0, 0);

            return false;
        }
    }

    public class ScholarStaffMetabolt : MetaballSystem
    {
        public override void DrawToTarget(SpriteBatch spriteBatch)
        {
            foreach (Projectile proj in Main.projectile.Where(n => n.ModProjectile is ScholarStaffBolt && n.active))
            {
                Color glowColor = Color.MediumTurquoise;
                glowColor.A = 0;

                List<Vector2> strip0 = new List<Vector2>();
                List<Vector2> strip1 = new List<Vector2>();
                List<float> rot0 = new List<float>();
                List<float> rot1 = new List<float>();

                float stripLength = 48;
                float t = proj.localAI[0] * 1.6f;
                for (int i = 0; i < (int)stripLength; i++)
                {
                    float rad = 10f * proj.scale + (float)Math.Sin(t * 0.1f % MathHelper.TwoPi);
                    Vector2 offRot0 = new Vector2(rad * (float)Math.Sqrt(Utils.GetLerpValue(stripLength, stripLength * 0.5f, i, true)), 0).RotatedBy(t * 0.1f + i / stripLength * 4f);
                    Vector2 offset0 = new Vector2(offRot0.X * proj.direction, offRot0.Y * 0.4f).RotatedBy(proj.rotation + t * 0.05f * proj.direction);
                    strip0.Add(offset0);
                    rot0.Add(offset0.ToRotation() - MathHelper.PiOver2);

                    Vector2 offRot1 = new Vector2(rad, 0).RotatedBy(t * 0.15f + i / stripLength * 5f + MathHelper.Pi / 2f);
                    Vector2 offset1 = new Vector2(offRot1.X * 0.6f, offRot1.Y * -proj.direction).RotatedBy(proj.rotation + t * 0.05f * proj.direction + MathHelper.TwoPi);
                    strip1.Add(offset1);
                    rot1.Add(offset1.ToRotation() - MathHelper.PiOver2);
                }

                Effect shader = ModContent.Request<Effect>($"{nameof(BlockVanity)}/Assets/Effects/ScholarEnergyTrail", AssetRequestMode.ImmediateLoad).Value;
                shader.Parameters["transformMatrix"].SetValue(Main.GameViewMatrix.NormalizedTransformationmatrix);
                shader.Parameters["uStreak0"].SetValue(TextureAssets.Extra[194].Value);
                //shader.Parameters["uStreak0"].SetValue(texture.Value);
                shader.Parameters["uColor"].SetValue(glowColor.ToVector4() * 1.1f);
                shader.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly * 2f % 1f);

                shader.CurrentTechnique.Passes[0].Apply();

                VertexStrip lat = new VertexStrip();
                VertexStrip vert = new VertexStrip();

                lat.PrepareStrip(strip0.ToArray(), rot0.ToArray(), (_) => glowColor, (float p) => Utils.GetLerpValue(1f, 0.6f, p, true) * Utils.GetLerpValue(0, 0.5f, p, true) * 16f, (proj.Center - Main.screenPosition) / 2f, strip0.Count, true);
                vert.PrepareStrip(strip1.ToArray(), rot1.ToArray(), (_) => glowColor, (float p) => Utils.GetLerpValue(1f, 0.6f, p, true) * Utils.GetLerpValue(0, 0.5f, p, true) * 16f, (proj.Center - Main.screenPosition) / 2f, strip1.Count, true);
                lat.DrawTrail();
                vert.DrawTrail();

                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            }
        }

        private void DrawBolts(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            orig(self);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null);
            Main.spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2f, 0, 0);
            Main.spriteBatch.End();
        }

        public override void Load()
        {
            On.Terraria.Main.DrawProjectiles += DrawBolts;
        }
    }
}
