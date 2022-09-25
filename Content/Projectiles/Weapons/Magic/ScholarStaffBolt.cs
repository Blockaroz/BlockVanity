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
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            int target = Projectile.FindTargetWithLineOfSight();
            if (target > -1)
            {
                float speed = 18f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, new Vector2(speed, 0).RotatedBy(Projectile.AngleTo(Main.npc[target].Center)), 0.05f);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity.SafeNormalize(Vector2.Zero) * speed, 0.3f);
            }

            if (Main.rand.NextBool(4))
            {
                Color particleColor = Color.Lerp(Color.DodgerBlue, Color.LightCyan, Main.rand.NextFloat(0.5f));
                particleColor.A = 0;
                Vector2 particleVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedByRandom(0.3f) * Main.rand.NextFloat(5f);
                Particle pixel = Particle.NewParticle(Particle.ParticleType<MagicPixelParticle>(), Projectile.Center + Main.rand.NextVector2Circular(15, 15), particleVelocity, particleColor, 1f);
                pixel.emit = true;
            }

            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.3f);
        }

        public override void Kill(int timeLeft)
        {
            SoundStyle hitSound = SoundID.DD2_GoblinBomb;
            hitSound.MaxInstances = 0;
            hitSound.PitchVariance = 0.2f;
            hitSound.Pitch = 0.3f;
            SoundEngine.PlaySound(hitSound, Projectile.Center);

            //for (int i = 0; i < Main.rand.Next(15, 20); i++)
            //{
            //    Color particleColor = Color.Lerp(Color.DodgerBlue, Color.LightCyan, Main.rand.NextFloat(0.5f));
            //    particleColor.A = 0;
            //    Particle pixel = Particle.NewParticle(Particle.ParticleType<MagicPixelParticle>(), Projectile.Center, Projectile.velocity * 0.01f + Main.rand.NextVector2Circular(3, 3), particleColor, 1f + Main.rand.NextFloat(2f));
            //    pixel.emit = true;
            //}

            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.5f);

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);

            Color glowColor = Color.DeepSkyBlue;
            glowColor.A = 20;
            Color deepGlowColor = Color.DodgerBlue;
            deepGlowColor.A = 0;

            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, deepGlowColor * 0.2f, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 1.3f, 0, 0);
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.9f, 0, 0);

            return false;
        }
    }
}
