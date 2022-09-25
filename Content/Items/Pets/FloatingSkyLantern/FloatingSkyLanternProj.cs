using BlockVanity.Common.Players;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.FloatingSkyLantern
{
    public class FloatingSkyLanternProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Floating Sky Lantern");

            Main.projFrames[Type] = 1;
            Main.projPet[Type] = true;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.LightPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.manualDirectionChange = true;
        }

        private Player Player => Main.player[Projectile.owner];

        public override void AI()
        {
            const float rotateFactor = 11f;

            float cos = (float)Math.Cos(Projectile.localAI[0] * 0.2f);
            Vector2 calcPos = new Vector2(-Player.direction * 50, -25 + cos);
            Vector2 vector = Player.MountedCenter + calcPos - Projectile.Center;
            float distance = Vector2.Distance(Projectile.Center, Player.MountedCenter + calcPos);

            if (!Player.active || Player.dead || !Player.GetModPlayer<MiscEffectPlayer>().floatingSkyLanternPet)
                Projectile.Kill();

            if (distance > 1000)
                Projectile.Center = Player.MountedCenter + calcPos;

            if (distance < 1f)
                Projectile.velocity *= 0.25f;

            if (vector != Vector2.Zero)
            {
                if (vector.Length() < 0.004f)
                    Projectile.velocity = vector;
                else
                    Projectile.velocity = vector * 0.1f;
            }

            Projectile.position += new Vector2(0, cos * 0.5f);

            // Rotation adapted from flying pets

            if (Projectile.velocity.Length() > 1f)
            {
                float value = Projectile.velocity.X * 0.02f +
                              Projectile.velocity.Y * Projectile.spriteDirection * 0.02f;

                if (Math.Abs(Projectile.rotation - value) >= MathHelper.Pi)
                {
                    if (value < Projectile.rotation)
                        Projectile.rotation -= MathHelper.TwoPi;
                    else
                        Projectile.rotation += MathHelper.TwoPi;
                }

                Projectile.rotation = (Projectile.rotation * (rotateFactor - 1f) + value) / rotateFactor;
            }
            else
            {
                if (Projectile.rotation > MathHelper.Pi)
                    Projectile.rotation -= MathHelper.TwoPi;

                if (Projectile.rotation > -0.004f && Projectile.rotation < 0.004f)
                    Projectile.rotation = 0;
                else
                    Projectile.rotation *= 0.95f;
            }

            Projectile.spriteDirection = Projectile.Center.X < Player.Center.X ? 1 : -1;

            float sine = ((float)Math.Sin(Projectile.localAI[0] * 0.3f) + 1f) * 0.2f;
            Lighting.AddLight(Projectile.Center, Color.DarkOrange.ToVector3() * (sine + 1f));

            if (Player.miscCounter % 15 == 0 && Main.rand.NextBool(8))
            {
                Vector2 center = Projectile.position;
                float rand = 1f + Main.rand.NextFloat() * 0.5f;

                if (Main.rand.NextBool(2))
                    rand *= -1f;

                center += new Vector2(rand * -25f, -8f);

                Dust dust = Dust.NewDustDirect(center, Player.width, Player.height, DustID.Firefly, 0, 0, 100);
                dust.rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
                dust.velocity.X = rand * 0.2f;
                dust.noGravity = true;
                dust.customData = Projectile;
                dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cLight, Player);
            }

            Projectile.localAI[0] += 0.1f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            Asset<Texture2D> tailTexture = ModContent.Request<Texture2D>(Texture + "_Tail");
            SpriteEffects dir = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            ArmorShaderData shaderData = GameShaders.Armor.GetSecondaryShader(Player.cLight, Player);
            if (shaderData != null)
                shaderData.Apply(null);

            Vector2 tailPos = Projectile.Center + new Vector2(0, 10).RotatedBy(Projectile.rotation);
            float tailWind = (float)Math.Sin(Projectile.localAI[0] * 1.5f) * 0.15f;
            float airVelocity = Projectile.velocity.X + Math.Max(0, Projectile.velocity.Y) * Projectile.spriteDirection;
            Main.EntitySpriteDraw(tailTexture.Value, tailPos - Main.screenPosition, null, lightColor, Projectile.rotation + airVelocity * 0.05f + tailWind, tailTexture.Size() * new Vector2(0.5f, 0f), Projectile.scale, dir, 0);
            
            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, dir, 0);

            float sine = ((float)Math.Sin(Projectile.localAI[0] * 0.3f) + 1f);
            int shades = 4;
            Color shadeColor = lightColor * 0.1f * (0.5f + sine * 0.5f);
            shadeColor.A /= 2;
            for (int i = 0; i < shades; i++)
            {
                Vector2 offset = new Vector2(sine * 2f, 0).RotatedBy(MathHelper.TwoPi / shades * i);
                Main.EntitySpriteDraw(texture.Value, Projectile.Center + offset - Main.screenPosition, null, shadeColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, dir, 0);
            }

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return false;
        }
    }
}
