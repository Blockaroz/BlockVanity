using BlockVanity.Content.Dusts;
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
    private static SoundStyle HitSound;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 8;
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
        if (Projectile.frameCounter++ > 3)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= 8)
                Projectile.frame = 0;
        }

        //if (Projectile.localAI[0] % 5 == 0)
        //    ParticleEngine.particles.NewParticle(new PixelSpotParticle(EnergyColor with { A = 50 }, 60, true), Projectile.Center + Main.rand.NextVector2Circular(5, 5), Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.5f), 0f, Main.rand.NextFloat(1f, 1.5f));

        if (Main.rand.NextBool(20) || Projectile.localAI[0] % 40 == 0)
        {
            Dust pixelDust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), ModContent.DustType<PixelSpotDust>(), Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.5f), 0, EnergyColor with { A = 60 }, Main.rand.NextFloat(1.5f, 2.5f));
            pixelDust.fadeIn = 60;
        }

        if (Main.rand.NextBool(5))
            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10, 10), DustID.Smoke, Projectile.velocity.RotatedByRandom(0.5f) * Main.rand.NextFloat(0.5f), 160, Color.DarkCyan, Main.rand.NextFloat(1f, 2f));

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

        for (int i = 0; i < 14; i++)
        {
            Vector2 offset = Main.rand.NextVector2Circular(8, 8);
            Dust pixelDust = Dust.NewDustPerfect(Projectile.Center + offset, ModContent.DustType<PixelSpotDust>(), Projectile.velocity * 0.2f + offset * Main.rand.NextFloat(0.5f), 0, EnergyColor with { A = 60 }, Main.rand.NextFloat(1.5f, 3f));
            pixelDust.fadeIn = 80;
        }
        
        //for (int i = 0; i < 11; i++)
        //{
        //    Vector2 offset = Main.rand.NextVector2Circular(5, 5);
        //    ParticleEngine.particles.NewParticle(new PixelSpotParticle(EnergyColor with { A = 50 }, 100, true), Projectile.Center + offset, Projectile.velocity * 0.2f + offset * Main.rand.NextFloat(0.5f), 0f, Main.rand.NextFloat(1.5f, 3f));
        //}
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
