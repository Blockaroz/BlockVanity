using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles
{
    public class DeepSpore : Particle
    {
        private int variant;
        private float rotVelocity;
        private float life;
        private Vector2 nextVelocity;

        public override void OnSpawn()
        {
            variant = Main.rand.Next(2);
            rotation += Main.rand.NextFloat(MathHelper.TwoPi);
            rotVelocity = Main.rand.NextFloat(-0.1f, 0.1f);
            life = scale * 10;
        }

        public override void Update()
        {
            if (Main.rand.NextBool(8))
                nextVelocity = Main.rand.NextVector2Circular(1, 1);

            velocity = Vector2.Lerp(velocity, nextVelocity, 0.1f);
            rotation += rotVelocity * scale;

            life *= 0.96f;
            if (life < 5f * scale)
                scale *= 0.95f;
            scale *= 0.98f;

            if (emit)
                Lighting.AddLight(position, color.ToVector3() * 0.12f);

            if (life < 0 || scale < 0.05f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            Rectangle frame = texture.Frame(1, 2, 0, variant);
            Color drawColor = color.MultiplyRGBA(Lighting.GetColor((int)position.X / 16, (int)position.Y / 16));
            if (emit)
                drawColor = color;
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, frame, drawColor, rotation, frame.Size() * 0.5f, scale, 0, 0);
        }
    }
}
