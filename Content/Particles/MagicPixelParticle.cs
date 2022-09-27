using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles
{
    public class MagicPixelParticle : Particle
    {
        private float life;
        private Vector2[] oldPos;
        private float[] oldRot;

        public override void OnSpawn()
        {
            rotation += Main.rand.NextFloat(MathHelper.TwoPi);
            life = Main.rand.Next(9, 14) * scale;
            oldPos = new Vector2[16];
            oldRot = new float[16];
            velocity *= 1.5f;
        }

        public override void Update()
        {
            velocity *= 0.91f;
            rotation = velocity.ToRotation();

            velocity = Vector2.Lerp(velocity, velocity * 1.05f + Main.rand.NextVector2Circular(1, 1), 0.15f);

            for (int i = oldPos.Length - 1; i > 0; i--)
            {
                oldPos[i] = oldPos[i - 1];
                oldRot[i] = oldRot[i - 1];
            }
            oldPos[0] = position;
            oldRot[0] = rotation;

            life *= 0.9f;

            if (life < 0.8f)
                scale *= 0.8f;

            if (emit)
                Lighting.AddLight(position, color.ToVector3() * 0.4f * Utils.GetLerpValue(0f, 2f, life, true));

            if (life < 0.03f || scale < 0.2f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);

            Color drawColor = color * Utils.GetLerpValue(0f, 1f, life, true);

            for (int i = 1; i < oldPos.Length; i++)
            {
                Color trailColor = drawColor * (float)Math.Pow(Utils.GetLerpValue(oldPos.Length, 0, i, true), 2f) * 0.3f;
                trailColor.A /= 3;
                Vector2 trailStretch = new Vector2(oldPos[i].Distance(oldPos[i - 1]) + 0.05f, scale);
                spriteBatch.Draw(texture.Value, oldPos[i] - Main.screenPosition, null, trailColor, oldRot[i], texture.Size() * 0.5f, trailStretch, 0, 0);
            }

            spriteBatch.Draw(texture.Value, position - Main.screenPosition, null, drawColor, rotation, texture.Size() * 0.5f, scale, 0, 0);
        }
    }
}
