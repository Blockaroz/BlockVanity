using BlockVanity.Common.Graphics;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Projectiles.Weapons.Magic;

public class ScholarStaffBolt : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 8;
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
        Projectile.timeLeft = 300;
    }

    public ref float Speed => ref Projectile.ai[0];

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.spriteDirection = Utils.ToDirectionInt(Main.rand.NextBool());
        Projectile.frame = Main.rand.Next(8);
    }

    public override void AI()
    {
        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Speed;
        Projectile.localAI[0]++;
        if (Projectile.frameCounter++ > 2)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= 8)
                Projectile.frame = 0;
        }

        if (Projectile.localAI[0] % 7 == 0 || Main.rand.NextBool(9))
        {
            MagicSmokeParticle darkParticle = MagicSmokeParticle.pool.RequestParticle();
            darkParticle.Prepare(Projectile.Center + Main.rand.NextVector2Circular(8, 8), Projectile.velocity, Projectile.velocity.ToRotation() + Main.rand.NextFloat(-1f, 1f), Main.rand.Next(20, 40), Color.DarkCyan, Color.Black * 0.5f, 0.5f + Main.rand.NextFloat(0.5f));
            ParticleEngine.Particles.Add(darkParticle);

            MagicSmokeParticle particle = MagicSmokeParticle.pool.RequestParticle();
            particle.Prepare(Projectile.Center + Projectile.velocity, Projectile.velocity + Main.rand.NextVector2Circular(3, 3), Projectile.velocity.ToRotation() + Main.rand.NextFloat(-1f, 1f), Main.rand.Next(20, 30), Color.White with { A = 50 }, EnergyColor with { A = 0 }, 0.4f + Main.rand.NextFloat(0.4f));
            ParticleEngine.Particles.Add(particle);
        }

        if (Main.rand.NextBool(20) || Projectile.localAI[0] % 20 == 0)
        {
            PixelSpotParticle particle = PixelSpotParticle.pool.RequestParticle();
            particle.Prepare(Projectile.Center + Main.rand.NextVector2Circular(15, 15), Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.5f), 60, 0, Color.White with { A = 0 }, EnergyColor with { A = 60 }, 1.5f + Main.rand.NextFloat());
            ParticleEngine.Particles.Add(particle);
        }

        Projectile.scale = 1.125f;

        Projectile.rotation = Projectile.velocity.X * 0.005f;
        Lighting.AddLight(Projectile.Center, EnergyColor.ToVector3() * 0.33f);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) => true;

    public override void OnKill(int timeLeft)
    {
        SoundStyle hitSound = SoundID.Item101.WithPitchOffset(0.5f).WithVolumeScale(0.6f);
        hitSound.MaxInstances = 0;
        hitSound.PitchVariance = 0.3f;
        SoundEngine.PlaySound(hitSound, Projectile.Center);

        for (int i = 0; i < 10; i++)
        {
            PixelSpotParticle particle = PixelSpotParticle.pool.RequestParticle();
            Vector2 offset = Main.rand.NextVector2Circular(8, 8);
            particle.Prepare(Projectile.Center + offset, Projectile.velocity * Main.rand.NextFloat(0.4f) + offset * 0.2f, Main.rand.Next(40, 80), 0, Color.White with { A = 0 }, EnergyColor with { A = 60 }, Main.rand.NextFloat(1.5f, 3f));
            ParticleEngine.Particles.Add(particle);
        }

        for (int i = 0; i < 3; i++)
        {
            MagicSmokeParticle burstParticle = MagicSmokeParticle.pool.RequestParticle();
            Vector2 particleVelocity = Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(2, 2);
            burstParticle.Prepare(Projectile.Center, particleVelocity, particleVelocity.ToRotation(), Main.rand.Next(25, 30), Color.White with { A = 50 }, EnergyColor with { A = 0 }, 0.5f + Main.rand.NextFloat());
            ParticleEngine.Particles.Add(burstParticle);
        }
    }

    public static readonly Color EnergyColor = new Color(22, 224, 214);

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Texture2D glow = AllAssets.Textures.Glow[0].Value;
        Rectangle frame = texture.Frame(1, 8, 0, Projectile.frame);

        lightColor = EnergyColor * 0.5f;
        SpriteEffects effects = Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : 0;

        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), lightColor with { A = 50 }, -Projectile.localAI[0] * 0.07f, glow.Size() * 0.5f, 0.4f * Projectile.scale, effects, 0);

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, effects, 0);
        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), lightColor with { A = 0 } * 0.3f, Projectile.localAI[0] * 0.03f, glow.Size() * 0.5f, 0.7f * Projectile.scale, effects, 0);

        return false;
    }
}