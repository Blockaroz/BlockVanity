using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using BlockVanity.Content.Particles;
using ParticleEngine;
using static System.Formats.Asn1.AsnWriter;

namespace BlockVanity.Content.Projectiles.Weapons.Magic
{
    public class ScholarStaffHeld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
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

        private Player Player => Main.player[Projectile.owner];

        private float MaxTime => Player.itemAnimationMax;

        private float SwingEase(float x)
        {
            float[] curve = new float[4];
            curve[0] = 0.05f * (float)Math.Pow(x, 0.4f);
            curve[1] = (float)Math.Pow(3f * x - 0.3f, 3f) + 0.05f;
            curve[2] = 5f * (float)Math.Pow(2f * x - 1f, 3f) + 0.55f;
            curve[3] = 7f * (float)Math.Pow(x - 1f, 3f) + 0.98f;

            if (x < 0)
                return 0;
            else if (x < 0.3f)
                return MathHelper.Lerp(curve[0], curve[1], Utils.GetLerpValue(0.2f, 0.3f, x, true));
            else if (x < 0.4f)
                return MathHelper.Lerp(curve[1], curve[2], Utils.GetLerpValue(0.3f, 0.4f, x, true));
            else if (x < 0.7f)
                return MathHelper.Lerp(curve[2], curve[3], Utils.GetLerpValue(0.6f, 0.7f, x, true));
            else if (x < 1f)
                return MathHelper.Lerp(curve[3], 1f, Utils.GetLerpValue(0.7f, 1f, x, true));

            return 1f;
        }

        public ref float Time => ref Projectile.ai[0];

        private float swingProgress;

        public override void AI()
        {
            float totalAngle = MathHelper.ToRadians(270);
            bool noUse = false;

            Player.heldProj = Projectile.whoAmI;
            Projectile.timeLeft = 5;
            Player.SetDummyItemTime(5);
            Player.ChangeDir(Math.Sign(Projectile.velocity.X));

            if (Time == -1)
            {
                Projectile.velocity = Player.DirectionTo(Main.MouseWorld) * 5f;
                Projectile.spriteDirection *= -1;

            }
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Player.DirectionTo(Main.MouseWorld) * 5f, 0.05f);

            Time++;

            swingProgress = SwingEase(Utils.GetLerpValue(1, MaxTime, Time + 1, true));
            float swingRot = Projectile.velocity.ToRotation() + (swingProgress - 0.51f) * totalAngle * Player.direction * Projectile.spriteDirection * (int)Player.gravDir;

            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, swingRot * (int)Player.gravDir - MathHelper.PiOver2);
            Projectile.Center = Player.RotatedRelativePointOld(Player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, swingRot - MathHelper.PiOver2), false);
            Projectile.rotation = swingRot;
           
            Vector2 scale = new Vector2(1f + (swingProgress * (1f - swingProgress)) * 2f) * Projectile.scale;
            Vector2 crystalPos = Projectile.Center + new Vector2(28).RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 + MathHelper.PiOver4) * scale;

            if (Time == (int)(MaxTime * 0.45f))
            {
                SoundStyle sound = SoundID.DD2_SkyDragonsFuryShot;
                sound.MaxInstances = 0;
                sound.PitchVariance = 0.3f;
                sound.Pitch = 1f - (MaxTime / 30f);
                SoundEngine.PlaySound(sound, Projectile.Center);

                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), crystalPos, crystalPos.DirectionTo(Main.MouseWorld) * 18f, ModContent.ProjectileType<ScholarStaffBolt>(), Projectile.damage, Projectile.knockBack, Player.whoAmI);
            }

            if (Time == (int)(MaxTime * 0.5f))
            {
                for (int i = 0; i < Main.rand.Next(5, 10); i++)
                {
                    Color particleColor = Color.Lerp(Color.Lerp(Color.DodgerBlue, Color.LightCyan, Main.rand.NextFloat(0.5f)), Color.DimGray * 0.2f, Main.rand.Next(2) * 0.7f);
                    particleColor.A = 0;
                    Vector2 particleVelocity = Projectile.velocity * 0.1f - Vector2.UnitY.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.5f, 3f);
                    Particle pixel = Particle.NewParticle(Particle.ParticleType<MagicPixelParticle>(), crystalPos + Main.rand.NextVector2Circular(15, 15), particleVelocity, particleColor, 1f + Main.rand.NextFloat(0.5f));
                    pixel.emit = true;
                }
            }

            if (Time > MaxTime - 1)
            {
                Time = -1;
                if (!Player.channel)
                    noUse = true;
            }

            if (!Player.active || Player.dead || Player.CCed || noUse)
                Projectile.Kill();

            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.2f);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => false;

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);

            int dir = Player.direction * (int)Player.gravDir;
            SpriteEffects effects = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            float upHandle = 0.1f;
            Vector2 origin = new Vector2(dir > 0 ? upHandle : 1f - upHandle, 1f - upHandle) * texture.Size();

            Vector2 scale = new Vector2(1f + (swingProgress * (1f - swingProgress)) * 2f) * Projectile.scale;

            float rotation = Projectile.rotation + MathHelper.PiOver2 - MathHelper.PiOver4 * dir;
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.White, 0.3f), rotation, origin, scale, effects, 0);

            DrawSoftGlow(lightColor);
            DrawFlare();

            return false;
        }

        private void DrawSoftGlow(Color lightColor)
        {
            Asset<Texture2D> glow = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/SoftGlow");

            Vector2 scale = new Vector2(1f + (swingProgress * (1f - swingProgress)) * 2f) * Projectile.scale;
            float rotation = Projectile.rotation - MathHelper.PiOver2 + MathHelper.PiOver4;// * Player.direction * (int)Player.gravDir;
            Vector2 crystalPos = Projectile.Center + new Vector2(25).RotatedBy(rotation) * scale;
            Vector2 stretch = new Vector2(1.1f, 1f) * scale * 0.6f;
            Color glowColor = Color.DodgerBlue;
            glowColor.A = 0;
            Main.EntitySpriteDraw(glow.Value, crystalPos - Main.screenPosition, null, glowColor * 0.1f, Projectile.rotation, glow.Size() * 0.5f, stretch * 1.5f, 0, 0);
            Main.EntitySpriteDraw(glow.Value, crystalPos - Main.screenPosition, null, glowColor * 0.8f, Projectile.rotation, glow.Size() * 0.5f, stretch * 0.5f, 0, 0);
        }

        private void DrawFlare()
        {
            Asset<Texture2D> star = TextureAssets.Extra[98];

            float fade = Utils.GetLerpValue(MaxTime * 0.3f, MaxTime * 0.5f, Time, true) * Utils.GetLerpValue(MaxTime * 0.7f, MaxTime * 0.5f, Time, true);
            Vector2 scale = new Vector2(1f + (swingProgress * (1f - swingProgress)) * 2f) * Projectile.scale;
            float rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2 + MathHelper.PiOver4;// * Player.direction * (int)Player.gravDir;
            Vector2 crystalPos = Projectile.Center + new Vector2(28).RotatedBy(rotation) * scale;
            Color glowColor = Color.DeepSkyBlue * 0.4f;
            glowColor.A = 0;

            Main.EntitySpriteDraw(star.Value, crystalPos - Main.screenPosition, null, glowColor, Projectile.rotation, star.Size() * 0.5f, new Vector2(1f, 1f) * fade, 0, 0);
            Main.EntitySpriteDraw(star.Value, crystalPos - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, star.Size() * 0.5f, new Vector2(0.4f, 0.8f) * fade, 0, 0);

            Main.EntitySpriteDraw(star.Value, crystalPos - Main.screenPosition, null, glowColor, Projectile.rotation + MathHelper.PiOver2, star.Size() * 0.5f, new Vector2(0.4f, 1.5f) * fade, 0, 0);
            Main.EntitySpriteDraw(star.Value, crystalPos - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation + MathHelper.PiOver2, star.Size() * 0.5f, new Vector2(0.3f, 0.5f) * fade, 0, 0);

        }
    }
}
