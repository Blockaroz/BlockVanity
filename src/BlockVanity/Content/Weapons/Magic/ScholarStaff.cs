using BlockVanity.Common.Graphics;
using BlockVanity.Common.UI;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Weapons.Magic;

public class ScholarStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.gunProj[Type] = true;
        ItemID.Sets.CanBePlacedOnWeaponRacks[Type] = true;
        BlockVanity.Sets.ItemDoesNotPayMana[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 34;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.channel = true;
        Item.noUseGraphic = true;
        Item.noMelee = true;

        Item.damage = 60;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 5;
        Item.knockBack = 8f;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.buyPrice(0, 2);
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shoot = ModContent.ProjectileType<ScholarStaffHeld>();
        Item.shootSpeed = 8f;
        Item.autoReuse = true;
    }

    public override Color? GetAlpha(Color lightColor) => Color.Lerp(Color.White, lightColor * 1.5f, 0.9f) with { A = 180 };
}

public class ScholarStaffHeld : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 34;
        Projectile.height = 34;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.DamageType = DamageClass.Magic;
    }

    private ref Player Player => ref Main.player[Projectile.owner];

    private float MaxTime => MathHelper.Max(Player.itemAnimationMax, 4);

    public ref float Time => ref Projectile.ai[0];
    public ref float Charge => ref Projectile.ai[1];

    public float swingProgress;

    public const float TOTAL_ANGLE = 240;

    public override void AI()
    {
        float totalAngleRadians = MathHelper.ToRadians(TOTAL_ANGLE);

        if (!Player.active || Player.dead || Player.CCed)
            Projectile.Kill();

        Player.heldProj = Projectile.whoAmI;
        Projectile.timeLeft = 2;
        Player.SetDummyItemTime(2);
        Player.ChangeDir(Math.Sign(Projectile.velocity.X));

        float realCharge = Math.Clamp(Charge - 20, 0, 50) / 50f;
        int count = (int)Math.Floor(realCharge * 2f) + 1;
        int manaNeeded = Player.GetManaCost(Player.HeldItem) * count;

        if (Player.channel && Player.CheckMana(manaNeeded, false))
        {
            Time = 0;

            if (Charge < 70)
                Charge++;

            if (Charge == 69)
            {
                Charge = 70;
                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Player.Center);
            }

            if (Charge > 10)
            {

            }
        }

        if (Time == 0 || Time == (int)(MaxTime * 0.4f))
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.velocity = Player.DirectionTo(Main.MouseWorld) * 5f;
                Projectile.netUpdate = true;
            }
        }

        swingProgress = Utils.GetLerpValue(MaxTime * 0.2f, MaxTime * 0.8f, Time, true);

        float startRotation = -MathHelper.Pi / 9f;

        if (Time < (int)(MaxTime * 0.2f))
        {
            Projectile.rotation = startRotation * Player.direction * Player.gravDir + MathHelper.PiOver2 - MathHelper.PiOver2 * Player.gravDir;
            Player.bodyFrame.Y = Player.bodyFrame.Height;
        }
        else
        {
            float rotationForHand = MathHelper.Lerp(MathHelper.Pi / 9f, new Vector2(Math.Abs(Projectile.velocity.X), Projectile.velocity.Y * Player.gravDir).ToRotation() + MathHelper.Pi / 2f, MathF.Sqrt(Utils.GetLerpValue(MaxTime * 0.4f, MaxTime, Time, true)));
            float rotation = MathHelper.Lerp(-MathHelper.Pi / 9f, new Vector2(Math.Abs(Projectile.velocity.X), Projectile.velocity.Y * Player.gravDir).ToRotation() + MathHelper.Pi / 2f, MathF.Sqrt(Utils.GetLerpValue(MaxTime * 0.4f, MaxTime * 0.5f, Time, true)));
            Projectile.rotation = rotation * Player.direction * Player.gravDir + MathHelper.PiOver2 - MathHelper.PiOver2 * Player.gravDir;
            Player.SetBodyFrameFromRotation(rotationForHand);
        }

        Projectile.Center = Player.HandPosition.Value - new Vector2(0, 4 * Player.gravDir) - Projectile.velocity;

        Vector2 scale = new Vector2(1f + (swingProgress * (1f - swingProgress)) * 2f) * Projectile.scale;
        Vector2 staffEndPos = Projectile.Center + new Vector2(30, 0).RotatedBy(Projectile.velocity.ToRotation()) * scale;

        if (Time == (int)(MaxTime * 0.5f))
        {
            if (count > 2)
            {
                SoundStyle sound = SoundID.Item122;
                sound.MaxInstances = 0;
                sound.PitchVariance = 0.1f;
                SoundEngine.PlaySound(sound, Projectile.Center);
            }
            else if (count > 1)
            {
                SoundStyle sound = SoundID.Item158;
                sound.MaxInstances = 0;
                sound.PitchVariance = 0.1f;
                SoundEngine.PlaySound(sound, Projectile.Center);
            }
            else
            {
                SoundStyle sound = SoundID.Item158;
                sound.MaxInstances = 0;
                sound.PitchVariance = 0.1f;
                SoundEngine.PlaySound(sound, Projectile.Center);
            }

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.velocity = Player.DirectionTo(Main.MouseWorld) * 5f;

                Vector2 shootPoint = staffEndPos;

                if (!Collision.CanHitLine(Player.Center, 0, 0, shootPoint, 0, 0))
                    shootPoint = Player.Center;

                Vector2 boltDirection = shootPoint.DirectionTo(Main.MouseWorld);

                Player.CheckMana(manaNeeded, true);

                for (int i = 0; i < count; i++)
                {
                    float viscosity = MathF.Sin(i * MathHelper.Pi / (count - 1f));
                    if (count < 2)
                        viscosity = 0f;
                    float spread = realCharge / 6f;
                    int damage = (int)(Projectile.damage * (1f + realCharge));
                    Vector2 boltVelocity = boltDirection.RotatedBy(spread * (count > 1 ? (Utils.GetLerpValue(0, count - 1, i, true) - 0.5f) : 0)) * (1 + 0.5f * viscosity);
                    Projectile bolt = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), shootPoint, boltVelocity, ModContent.ProjectileType<ScholarStaffBolt>(), Projectile.damage, Projectile.knockBack, Player.whoAmI, ai0: Main.rand.NextFloat(8, 10));
                    bolt.localAI[0] = Main.rand.Next(-2, 2);
                    bolt.scale *= 1f + 0.5f * viscosity;
                }

                Projectile.netUpdate = true;
            }
        }

        if (Time >= (int)(MaxTime * 0.6f) && Time < (int)(MaxTime * 0.8f))
        {
            PixelEmber particle = PixelEmber.RequestNew(staffEndPos, Projectile.velocity.RotatedByRandom(0.7f) * Main.rand.NextFloat(0.2f, 0.4f), 50, 0, Color.White with { A = 0 }, ScholarStaffBolt.EnergyColor with { A = 60 }, Main.rand.NextFloat(1f, 3f));
            ParticleEngine.Particles.Add(particle);

            for (int i = 0; i < 2; i++)
            {
                Dust.NewDustPerfect(staffEndPos, DustID.Smoke, Projectile.velocity.RotatedByRandom(0.7f) * Main.rand.NextFloat(0.5f), 100, Color.LightGray, Main.rand.NextFloat(1f, 2f));
            }
        }

        if (Time > (int)MaxTime)
        {
            Player.SetDummyItemTime(0);
            Projectile.Kill();
        }

        Time++;

        Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.1f);
    }

    public override bool? CanCutTiles() => false;

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => false;

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        int dir = Player.direction * (int)Player.gravDir;
        SpriteEffects effects = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Rectangle frame = texture.Frame(2, 1, 0, 0);
        Rectangle glowFrame = texture.Frame(2, 1, 1, 0);

        Vector2 scale = new Vector2(1f + swingProgress * (1f - swingProgress)) * Projectile.scale;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, Color.Lerp(lightColor, Color.White, 0.4f), Projectile.rotation, new Vector2(0.5f, 0.8f) * frame.Size(), scale, effects, 0);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, glowFrame, Color.White with { A = 150 } * swingProgress, Projectile.rotation, new Vector2(0.5f, 0.8f) * glowFrame.Size(), scale, effects, 0);

        return false;
    }
}

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
        Projectile.scale = 1.125f;
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
        if (Projectile.localAI[0] % 3 == 0)
        {
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] >= 12)
            {
                Projectile.localAI[1] = 0;
                sparkRotation = Main.rand.Next(-4, 4);
            }
        }

        if (Projectile.localAI[0] % 8 == 0 || Main.rand.NextBool(8))
        {
            MagicSmokeParticle darkParticle = MagicSmokeParticle.RequestNew(
                Projectile.Center + Main.rand.NextVector2Circular(8, 8),
                Projectile.velocity + Main.rand.NextVector2Circular(3, 3),
                Projectile.velocity.ToRotation() + Main.rand.NextFloat(-1f, 1f),
                Main.rand.Next(30, 40),
                (Color.Beige * 0.3f) with { A = 120 },
                Color.Beige with { A = 20 } * 0.1f,
                0.4f + Main.rand.NextFloat(0.4f));
            darkParticle.LightAffected = true;
            ParticleEngine.Particles.Add(darkParticle);
        }

        if (Projectile.localAI[0] % 10 == 0 || Main.rand.NextBool(8))
        {
            MagicSmokeParticle particle = MagicSmokeParticle.RequestNew(
                Projectile.Center + Projectile.velocity,
                Projectile.velocity + Main.rand.NextVector2Circular(3, 3),
                Projectile.velocity.ToRotation() + Main.rand.NextFloat(-1f, 1f),
                Main.rand.Next(20, 30),
                Color.White with { A = 50 },
                EnergyColor with { A = 0 },
                0.4f + Main.rand.NextFloat(0.3f));
            ParticleEngine.Particles.Add(particle);
        }

        if (Main.rand.NextBool(20) || Projectile.localAI[0] % 20 == 0)
        {
            PixelEmber particle = PixelEmber.RequestNew(Projectile.Center + Main.rand.NextVector2Circular(15, 15), Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.5f), 60, 0, Color.White with { A = 0 }, EnergyColor with { A = 60 }, 1.5f + Main.rand.NextFloat());
            ParticleEngine.Particles.Add(particle);
        }

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
            Vector2 offset = Main.rand.NextVector2Circular(8, 8);
            PixelEmber particle = PixelEmber.RequestNew(Projectile.Center + offset, Projectile.velocity * Main.rand.NextFloat(0.4f) + offset * 0.2f, Main.rand.Next(40, 80), 0, Color.White with { A = 0 }, EnergyColor with { A = 60 }, Main.rand.NextFloat(1.5f, 3f));
            ParticleEngine.Particles.Add(particle);
        }

        for (int i = 0; i < 3; i++)
        {
            Vector2 particleVelocity = Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(2, 2);
            MagicSmokeParticle burstParticle = MagicSmokeParticle.RequestNew(Projectile.Center, particleVelocity, particleVelocity.ToRotation(), Main.rand.Next(25, 30), Color.White with { A = 50 }, EnergyColor with { A = 0 }, 0.6f + i / 3f);
            ParticleEngine.Particles.Add(burstParticle);
        }
    }

    public static readonly Color EnergyColor = new Color(22, 224, 214);

    public static Asset<Texture2D> sparksTexture;

    public override void Load()
    {
        sparksTexture = ModContent.Request<Texture2D>(Texture + "Sparks");
    }

    private int sparkRotation;

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Texture2D glow = Assets.Textures.Glow[0].Value;
        Rectangle frame = texture.Frame(1, 8, 0, Projectile.frame);

        lightColor = EnergyColor * 0.5f;
        SpriteEffects effects = Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : 0;

        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), lightColor with { A = 50 }, -Projectile.localAI[0] * 0.07f, glow.Size() * 0.5f, 0.4f * Projectile.scale, effects, 0);

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, effects, 0);

        Rectangle sparksFrame = sparksTexture.Frame(1, 12, 0, (int)Math.Floor(Projectile.localAI[1]));
        SpriteEffects sparksEffect = sparkRotation < 0 ? SpriteEffects.FlipHorizontally : 0;

        Main.EntitySpriteDraw(sparksTexture.Value, Projectile.Center - Main.screenPosition, sparksFrame, Color.LightCyan with { A = 0 }, Projectile.rotation * 0.5f + sparkRotation * MathHelper.PiOver2, sparksFrame.Size() * 0.5f, Projectile.scale * 0.9f, sparksEffect, 0);
        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), lightColor with { A = 0 } * 0.3f, Projectile.localAI[0] * 0.03f, glow.Size() * 0.5f, 0.7f * Projectile.scale, effects, 0);

        return false;
    }
}