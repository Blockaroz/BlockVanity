using System;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Projectiles.Weapons.Magic;

public class ScholarStaffProj : ModProjectile
{
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

    private float SwingEase(float x)
    {
        float[] curve = [
            -0.1f * (float)Math.Pow(x, 0.4f),
            (float)Math.Pow(2.8f * x - 0.3f, 4f) - 0.05f,
            3f * (float)Math.Pow(2f * x - 1.1f, 3f) + 0.53f,
            2f * (float)Math.Pow(x - 0.5f, 2f) + 0.53f,
        ];
        if (x < 0)
        {
            return 0;
        }
        else if (x < 0.3f)
        {
            return MathHelper.Lerp(curve[0], curve[1], Utils.GetLerpValue(0.15f, 0.3f, x, true));
        }
        else if (x < 0.4f)
        {
            return MathHelper.Lerp(curve[1], curve[2], Utils.GetLerpValue(0.3f, 0.4f, x, true));
        }
        else if (x < 0.6f)
        {
            return MathHelper.Lerp(curve[2], curve[3], Utils.GetLerpValue(0.5f, 0.6f, x, true));
        }
        else if (x < 1f)
        {
            return MathHelper.Lerp(curve[3], 1f, Utils.GetLerpValue(0.8f, 1f, x, true));
        }

        return 1f;
    }

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

        if (Player.channel)
        {
            Time = 0;

            if (++Charge > 50)
                Charge = 50;

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
            float rotationForHand = MathHelper.Lerp(-MathHelper.Pi / 9f, new Vector2(Math.Abs(Projectile.velocity.X), Projectile.velocity.Y * Player.gravDir).ToRotation() + MathHelper.Pi / 2f, MathF.Sqrt(Utils.GetLerpValue(MaxTime * 0.3f, MaxTime, Time, true)));
            float rotation = MathHelper.Lerp(-MathHelper.Pi / 9f, new Vector2(Math.Abs(Projectile.velocity.X), Projectile.velocity.Y * Player.gravDir).ToRotation() + MathHelper.Pi / 2f, MathF.Sqrt(Utils.GetLerpValue(MaxTime * 0.4f, MaxTime * 0.5f, Time, true)));
            Projectile.rotation = rotation * Player.direction * Player.gravDir + MathHelper.PiOver2  - MathHelper.PiOver2 * Player.gravDir;
            Player.bodyFrame.Y = VanityUtils.BodyFrameArmFromRotation(Player, rotationForHand) * Player.bodyFrame.Height;
        }

        Projectile.Center = Player.HandPosition.Value - new Vector2(0, 4 * Player.gravDir) - Projectile.velocity;

        Vector2 scale = new Vector2(1f + (swingProgress * (1f - swingProgress)) * 2f) * Projectile.scale;
        Vector2 crystalPos = Projectile.Center + new Vector2(30, 0).RotatedBy(Projectile.velocity.ToRotation()) * scale;

        if (Time == (int)(MaxTime * 0.5f))
        {
            SoundStyle sound = SoundID.DD2_BetsyFireballImpact.WithPitchOffset(0.5f - Charge / 60f);
            sound.MaxInstances = 0;
            sound.PitchVariance = 0.1f;
            SoundEngine.PlaySound(sound, Projectile.Center);

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.velocity = Player.DirectionTo(Main.MouseWorld) * 5f;

                int count = (int)Math.Floor(Charge / 20) + 1;
                for (int i = 0; i < count; i++)
                {
                    if (i > 0)
                        Player.CheckMana(Player.GetManaCost(Player.HeldItem), true);

                    float inaccuracy = Charge / 300f;
                    int damage = (int)(Projectile.damage * (1f + Charge / 50f));
                    Projectile bolt = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), crystalPos, crystalPos.DirectionTo(Main.MouseWorld).RotatedByRandom(0.03f + inaccuracy * 0.1f).RotatedBy(inaccuracy * (count > 1 ? (Utils.GetLerpValue(0, count - 1, i, true) - 0.5f) : 0)), ModContent.ProjectileType<ScholarStaffBolt>(), Projectile.damage, Projectile.knockBack, Player.whoAmI);
                    bolt.ai[0] = Main.rand.NextFloat(0.6f, 1.5f);
                    bolt.localAI[0] = Main.rand.Next(-5, 5);
                }

                Projectile.netUpdate = true;
            }
        }

        if (Time > (int)(MaxTime * 0.4f) && Time < (int)(MaxTime * 0.8f))
            ParticleEngine.particles.NewParticle(new MagicTrailParticle(ScholarStaffBolt.EnergyColor with { A = 0 }, true), crystalPos + Main.rand.NextVector2Circular(6, 6), Main.rand.NextVector2Circular(2, 2) + Projectile.velocity * 0.3f, 0f, Main.rand.NextFloat(1f, 1.5f));

        if (Time > (int)MaxTime)
        {
            Player.SetDummyItemTime(0);
            Projectile.Kill();
        }

        Time++;

        Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.2f);
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => false;

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Texture2D glow = AllAssets.Textures.Glow[0].Value;

        int dir = Player.direction * (int)Player.gravDir;
        SpriteEffects effects = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Rectangle frame = texture.Frame(2, 1, 0, 0);
        Rectangle glowFrame = texture.Frame(2, 1, 1, 0);

        Vector2 scale = new Vector2(1f + swingProgress * (1f - swingProgress) * 1.6f) * Projectile.scale;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, Color.Lerp(lightColor, Color.White, 0.1f), Projectile.rotation, new Vector2(0.5f, 0.8f) * frame.Size(), scale, effects, 0);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, glowFrame, Color.White, Projectile.rotation, new Vector2(0.5f, 0.8f) * glowFrame.Size(), scale, effects, 0);

        Vector2 crystalPos = Projectile.Center + new Vector2(0, -25).RotatedBy(Projectile.rotation) * scale;

        Vector2 stretch = new Vector2(1f, 1.2f) * scale * 0.1f;

        Color glowColor = ScholarStaffBolt.EnergyColor * (0.2f + swingProgress * (1f - swingProgress) * 0.5f);
        glowColor.A = 0;

        Main.EntitySpriteDraw(glow, crystalPos - Main.screenPosition, glow.Frame(), glowColor, Projectile.rotation, glow.Size() * 0.5f, stretch * (2f + swingProgress * (1f - swingProgress) * 2f), effects, 0);

        return false;
    }
}
