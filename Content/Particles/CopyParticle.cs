using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles
{
    public class CopyParticle : Particle
    {
        private Asset<Texture2D> copyTexture;
        private float rotVelocity;

        public override void OnSpawn()
        {
            rotation += Main.rand.NextFloat(MathHelper.TwoPi);
            rotVelocity = Main.rand.NextFloat(-0.1f, 0.1f);
        }

        public override void Update()
        {
            velocity = Vector2.Lerp(velocity, Main.rand.NextVector2Circular(1, 1), 0.1f);
            velocity *= 0.98f;
            rotation += rotVelocity * scale;

            if (data is Asset<Texture2D> tex)
                copyTexture = tex;
            else
                Active = false;

            scale *= 0.96f;
            if (scale < 0.55f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (copyTexture != null)
            {
                Color drawColor = color.MultiplyRGBA(Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f)));
                if (emit)
                    drawColor = color;
                spriteBatch.Draw(copyTexture.Value, position - Main.screenPosition, null, drawColor * Utils.GetLerpValue(0.6f, 0.9f, scale, true), rotation, copyTexture.Size() * 0.5f, Utils.GetLerpValue(-0.4f, 0.8f, scale), 0, 0);
            }
        }
    }
}
