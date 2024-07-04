using System;
using BlockVanity;
using BlockVanity.Content.Projectiles.Weapons;
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

    private float MaxTime => Player.itemAnimationMax;

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

    public float swingProgress;

    public const float TOTAL_ANGLE = 240;

    public override void AI()
    {
        float totalAngleRadians = MathHelper.ToRadians(TOTAL_ANGLE);

        if (!Player.active || Player.dead || Player.CCed)
        {
            Projectile.Kill();
        }

        Player.heldProj = Projectile.whoAmI;
        Projectile.timeLeft = 5;
        Player.SetDummyItemTime(5);
        Player.ChangeDir(Math.Sign(Projectile.velocity.X));

        if (Time == 0)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.velocity = Player.DirectionTo(Main.MouseWorld) * 5f;
                Projectile.netUpdate = true;
            }
        }

        Projectile.velocity = Vector2.Lerp(Projectile.velocity, Player.DirectionTo(Main.MouseWorld) * 5f, 0.02f);

        swingProgress = SwingEase(Utils.GetLerpValue(1, MaxTime, Time + 1, true));
        float swingRot = Projectile.velocity.ToRotation() + (swingProgress - 0.51f) * totalAngleRadians * Player.direction * Projectile.spriteDirection * (int)Player.gravDir;

        //Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, swingRot * (int)Player.gravDir - MathHelper.PiOver2);
        Projectile.Center = Player.RotatedRelativePointOld(Player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, swingRot - MathHelper.PiOver2), false);
        Projectile.rotation = swingRot;

        Vector2 scale = new Vector2(1f + (swingProgress * (1f - swingProgress)) * 2f) * Projectile.scale;
        Vector2 crystalPos = Projectile.Center + new Vector2(28).RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 + MathHelper.PiOver4) * scale;

        if (Time == (int)(MaxTime * 0.33f))
        {
            SoundStyle sound = SoundID.DD2_DarkMageHealImpact;
            sound.MaxInstances = 0;
            sound.PitchVariance = 0.1f;
            sound.Pitch = 1.66f - (MaxTime / 40f);
            SoundEngine.PlaySound(sound, Projectile.Center);
            Player.CheckMana(5, true);
        }

        if (Time == (int)(MaxTime * 0.5f))
        {

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile bolt = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), crystalPos, crystalPos.DirectionTo(Main.MouseWorld).RotatedByRandom(0.04f), ModContent.ProjectileType<ScholarStaffBolt>(), Projectile.damage, Projectile.knockBack, Player.whoAmI);
                bolt.localAI[0] = Main.rand.Next(-5, 5);
                Projectile.netUpdate = true;
            }
        }

        if (Time > MaxTime)
        {
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

        float upHandle = 0.1f;
        Vector2 origin = new Vector2(dir > 0 ? upHandle : 1f - upHandle, 1f - upHandle) * texture.Size();

        Vector2 scale = new Vector2(1f + swingProgress * (1f - swingProgress) * 2f) * Projectile.scale;

        float rotation = Projectile.rotation + MathHelper.PiOver2 - MathHelper.PiOver4 * dir;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.White, 0.5f), rotation, origin, scale, effects, 0);

        Vector2 crystalPos = Projectile.Center + new Vector2(25).RotatedBy(Projectile.rotation - MathHelper.PiOver4) * scale;

        Vector2 stretch = new Vector2(1.1f, 1f) * scale * 0.1f;

        Color glowColor = (Color.Lerp(Color.Turquoise, Color.LightCyan, 0.33f) * 0.33f) with { A = 0 };

        Main.EntitySpriteDraw(glow, crystalPos - Main.screenPosition, null, glowColor * 0.3f, rotation, glow.Size() * 0.5f, stretch * 1.66f, effects, 0);
        Main.EntitySpriteDraw(glow, crystalPos - Main.screenPosition, null, glowColor, rotation, glow.Size() * 0.5f, stretch, effects, 0);

        return false;
    }
}
