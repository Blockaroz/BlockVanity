using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles
{
    public class AncientCrescent : Particle
    {
        private int variant;
        private float rotVelocity;
        private float life;

        public override void OnSpawn()
        {
            variant = Main.rand.Next(2);
            rotation += Main.rand.NextFloat(MathHelper.TwoPi);
            rotVelocity = Main.rand.NextFloat(0.3f, 1f) * Main.rand.NextBool().ToDirectionInt();
            life = Main.rand.Next(9, 14);
        }

        public override void Update()
        {
            velocity *= 0.98f;
            rotation += rotVelocity;

            life *= 0.96f;
            if (life < 8f)
                scale *= 0.8f;
            scale *= 0.95f;

            if (emit)
                Lighting.AddLight(position, color.ToVector3() * 0.12f);

            if (life < 0 || scale < 0.05f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            Rectangle frame = texture.Frame(1, 2, 0, variant);
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, frame, color, rotation, frame.Size() * 0.5f, scale, 0, 0);
        }
    }
}
