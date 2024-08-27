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
        Projectile.timeLeft = 300;
    }

    public ref float Speed => ref Projectile.ai[0];

    public override void AI()
    {
        float speed = 7f * Speed;

        Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity.SafeNormalize(Vector2.Zero) * speed, 0.16f);

        Projectile.localAI[0] += 0.5f + Speed * 0.5f;

        if (Main.rand.NextBool(35))
            ParticleEngine.particles.NewParticle(new MagicTrailParticle(EnergyColor with { A = 0 }, true, 6), Projectile.Center + Projectile.velocity * 0.6f + Main.rand.NextVector2Circular(10, 10), Projectile.velocity * 0.5f, 0f, Main.rand.NextFloat(1f, 2f));

        //if (Projectile.timeLeft % 5 == 0 || Main.rand.NextBool(15))

        Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.33f);
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
            Vector2 offset = Main.rand.NextVector2Circular(10, 8);
            ParticleEngine.particles.NewParticle(new MagicTrailParticle(EnergyColor with { A = 0 }, true), Projectile.Center + offset, offset * Main.rand.NextFloat(0.3f) - Vector2.UnitY * 0.2f, 0f, Main.rand.NextFloat(2f, 3f));
        }

        ParticleEngine.particles.NewParticle(new ScholarStaffExplosionParticle(EnergyColor with { A = 0 }, true), Projectile.Center, Vector2.Zero, 0f, Projectile.scale);
    }

    public static readonly Color EnergyColor = Color.Lerp(Color.DodgerBlue, Color.Turquoise, 0.6f);

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Texture2D glow = AllAssets.Textures.Glow[0].Value;

        Color color = EnergyColor with { A = 50 };

        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), Color.Black * 0.7f, 0f, glow.Size() * 0.5f, 0.4f * Projectile.scale, 0, 0);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(), color, Projectile.rotation, texture.Size() * 0.5f, 1f, 0, 0);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(), new Color(225, 255, 255, 0), Projectile.rotation, texture.Size() * 0.5f, 0.7f, 0, 0);
        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), color with { A = 0 }, 0f, glow.Size() * 0.5f, 0.35f * Projectile.scale, 0, 0);
        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), color with { A = 0 } * 0.3f, 0f, glow.Size() * 0.5f, 0.6f * Projectile.scale, 0, 0);

        return false;
    }
}
