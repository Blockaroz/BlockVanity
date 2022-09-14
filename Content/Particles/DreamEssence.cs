using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles
{
    public class DreamEssence : Particle
    {
        private float dotScale;
        private int variant;
        private int life;
        private float angularVelocity;

        public override void OnSpawn()
        {
            dotScale = Main.rand.NextFloat(0.3f, 1f);
            variant = Main.rand.Next(6);
            if (data is null)
                data = Main.rand.Next(50, 90);
            life = (int)data;
            angularVelocity = Main.rand.NextFloat(-0.1f, 0.1f);
            rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }

        public override void Update()
        {
            life--;

            if (emit)
                Lighting.AddLight(position, color.ToVector3() * 0.3f);

            rotation += angularVelocity;

            velocity += Main.rand.NextVector2Circular(1, 1) * 0.06f;

            velocity *= 0.8f + (Utils.GetLerpValue(0, (int)data * 0.8f, life, true) * 0.2f);

            if (life < 0 || scale < 0.1f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            Rectangle frame = texture.Frame(1, 6, 0, variant);

            Color glowColor = color;
            if (shader == null)
                glowColor.A = 0;
            float scaleFade = Utils.GetLerpValue(0, (int)data * 0.12f, life, true) * Utils.GetLerpValue((int)data, (int)data * 0.82f, life, true) * scale * 0.5f;

            spriteBatch.Draw(texture.Value, position - Main.screenPosition, frame, glowColor, rotation, frame.Size() * 0.5f, scaleFade, 0, 0);
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, texture.Frame(1, 6, 0, 5), new Color(glowColor.R, glowColor.G, glowColor.B, 0), rotation, frame.Size() * 0.5f, scaleFade * dotScale, 0, 0);
        }
    }
}
