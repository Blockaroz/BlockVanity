using System;
using Microsoft.Xna.Framework;
using Terraria;
using ParticleEngine;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Content.Particles
{
    public class SmokeWisp : Particle
    {
        private int life;
        private int lifeMax;
        private int frame;

        public override void OnSpawn()
        {
            life = 0;
            lifeMax = (int)(scale * 6 + Main.rand.Next(9, 13));
            scale *= Main.rand.NextFloat(0.9f, 1.1f);
            frame = Main.rand.Next(5);
            rotation = velocity.RotatedByRandom(0.2f).ToRotation();
        }

        public override void Update()
        {
            rotation = velocity.ToRotation();

            velocity = Vector2.Lerp(velocity, velocity + Main.rand.NextVector2Circular(1, 1), 0.03f);
            velocity *= 1.01f;

            scale *= Main.rand.NextFloat(0.97f, 1f);

            if (life++ > lifeMax || scale < 0.1f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            Rectangle variant = texture.Frame(1, 6, 0, frame);

            Color lightColor = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
            if (emit)
                lightColor = Color.White;

            float fade = Utils.GetLerpValue(0, 3 * scale, life, true) * Utils.GetLerpValue(lifeMax, lifeMax - 12 * scale, life, true);
            Vector2 stretch = new Vector2(1.5f, velocity.Length() * 0.5f + 0.5f) * scale;
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, variant, Color.Black * 0.01f * fade, rotation - MathHelper.PiOver2, variant.Size() * new Vector2(0.5f, 0.6f), stretch * 1.4f, 0, 0);
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, variant, color.MultiplyRGBA(lightColor) * fade, rotation - MathHelper.PiOver2, variant.Size() * new Vector2(0.5f, 0.7f), stretch, 0, 0);
        }
    }
}
