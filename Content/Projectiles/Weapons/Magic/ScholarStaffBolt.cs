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

        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Color particleColor = Color.Lerp(Color.DodgerBlue, Color.LightCyan, Main.rand.NextFloat(0.5f));
            particleColor.A = 0;
            Vector2 particleVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedByRandom(0.3f) * Main.rand.NextFloat(5f);
            Particle crescent = Particle.NewParticle(Particle.ParticleType<AncientCrescent>(), Projectile.Center + Main.rand.NextVector2Circular(11, 11), particleVelocity, particleColor, Main.rand.NextFloat(0.7f));
            crescent.emit = true;
            crescent.data = 15;

            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.3f);
        }

        public override void Kill(int timeLeft)
        {
            SoundStyle hitSound = SoundID.LiquidsWaterLava;
            hitSound.MaxInstances = 0;
            hitSound.PitchVariance = 0.3f;
            hitSound.Pitch = 0.6f;
            SoundEngine.PlaySound(hitSound, Projectile.Center);

            for (int i = 0; i < Main.rand.Next(20, 30); i++)
            {
                Color particleColor = Color.Lerp(Color.DodgerBlue, Color.LightCyan, Main.rand.NextFloat(0.5f));
                particleColor.A = 0;
                Particle crescent = Particle.NewParticle(Particle.ParticleType<AncientCrescent>(), Projectile.Center, Projectile.velocity * 0.1f + Main.rand.NextVector2Circular(5, 5), particleColor, Main.rand.NextFloat(0.7f));
                crescent.emit = true;
                crescent.data = 15;
            }

            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.5f);

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);

            Color glowColor = Color.DeepSkyBlue;
            glowColor.A /= 4;

            Vector2 stretch = new Vector2(Projectile.velocity.Length() * 0.015f + 0.9f, 1f) * Projectile.scale;
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, glowColor * 0.1f, Projectile.rotation, texture.Size() * 0.5f, stretch * 2f, 0, 0);
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, texture.Size() * 0.5f, stretch, 0, 0);
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, texture.Size() * 0.5f, stretch * 0.8f, 0, 0);
            return false;
        }
    }
}
