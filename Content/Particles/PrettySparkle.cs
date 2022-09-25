using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles
{
    public class PrettySparkle : Particle
    {
        private int life;

        public override void OnSpawn()
        {
            rotation = Main.rand.NextFloat(-0.05f, 0.05f);
        }

        public override void Update()
        {
            if (data is null)
                data = Main.rand.Next(70, 90) + (int)(scale * 2);
            if (life <= 0)
                life = (int)data;

            life--;

            if (emit)
                Lighting.AddLight(position, color.ToVector3() * 0.3f);

            velocity *= 0.8f + (Utils.GetLerpValue((int)data * 0.3f, (int)data * 0.8f, life, true) * 0.23f);

            if (life <= 0 || scale < 0.1f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);

            if (data != null)
            {
                float scaleFade = Utils.GetLerpValue(0, (int)data * 0.3f, life, true) * Utils.GetLerpValue((int)data, (int)data - 4, life, true) * scale;

                Color glowColor = color * 0.5f;
                glowColor.A /= 2;
                Color shineColor = Color.Lerp(color, Color.White, 0.8f * scaleFade) * 0.7f;
                shineColor.A = 0;

                spriteBatch.Draw(texture.Value, position - Main.screenPosition, null, glowColor, rotation, texture.Size() * 0.5f, scaleFade * new Vector2(1f, 1.5f), 0, 0);
                spriteBatch.Draw(texture.Value, position - Main.screenPosition, null, glowColor, rotation + MathHelper.PiOver2, texture.Size() * 0.5f, scaleFade * new Vector2(1f, 1.1f), 0, 0);
                spriteBatch.Draw(texture.Value, position - Main.screenPosition, null, shineColor, rotation, texture.Size() * 0.5f, scaleFade * new Vector2(0.5f, 0.8f), 0, 0);
                spriteBatch.Draw(texture.Value, position - Main.screenPosition, null, shineColor, rotation + MathHelper.PiOver2, texture.Size() * 0.5f, scaleFade * new Vector2(0.4f, 0.6f), 0, 0);

            }
        }
    }
}
