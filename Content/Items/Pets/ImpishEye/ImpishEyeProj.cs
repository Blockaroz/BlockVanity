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

namespace BlockVanity.Content.Items.Pets.ImpishEye
{
    public class ImpishEyeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Impish Eye");

            Main.projFrames[Type] = 1;
            Main.projPet[Type] = true;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
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
            if (!Player.active || Player.dead || !Player.GetModPlayer<MiscEffectPlayer>().impishEyePet)
                Projectile.Kill();

            Projectile.ai[0] += 0.33f * Player.direction;

            Vector2 homePos = Player.MountedCenter + new Vector2(80, 0).RotatedBy(Projectile.ai[0] * 0.2f);
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(homePos) * Projectile.Distance(homePos) * 0.07f, 0.1f);

            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;

            int frameSpeed = Projectile.velocity.Length() > 15 ? 2 : 4;
            if (Projectile.frameCounter++ > frameSpeed)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame > 3)
                    Projectile.frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            SpriteEffects dir = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Rectangle frame = texture.Frame(1, 4, 0, Projectile.frame);

            ArmorShaderData shaderData = GameShaders.Armor.GetSecondaryShader(Player.cLight, Player);
            if (shaderData != null)
                shaderData.Apply(null);

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)
            {
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size * 0.5f;
                float oldRot = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture.Value, oldPos - Main.screenPosition, frame, lightColor.MultiplyRGBA(new Color(40, 40, 40)) * Utils.GetLerpValue(8, 0, i, true) * 0.2f, oldRot, frame.Size() * new Vector2(0.8f, 0.5f), Projectile.scale * 1.4f, dir, 0);
            }

            Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, frame.Size() * new Vector2(0.8f, 0.5f), Projectile.scale, dir, 0);

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return false;
        }
    }
}
