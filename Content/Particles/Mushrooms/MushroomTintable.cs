using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles.Mushrooms
{
    public class MushroomTintable : Particle
    {
        private int type;

        public override void OnSpawn()
        {
            type = Main.rand.Next(3);
            rotation += Main.rand.NextFloat(-0.8f, 0.8f);
        }

        public override void Update()
        {
            scale *= 0.95f;
            if (scale < 0.2f)
                Active = false;

            if (emit)
                Lighting.AddLight(position, color.ToVector3() * 0.3f * scale);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> mushroom = ModContent.Request<Texture2D>(Texture);
            Rectangle frame = mushroom.Frame(4, 2, type, 0);
            Rectangle capFrame = mushroom.Frame(4, 2, type, 1);
            Vector2 origin = frame.Size() * new Vector2(0.5f, 0.9f);

            Color light = Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f));
            spriteBatch.Draw(mushroom.Value, position - Main.screenPosition, frame, light, rotation, origin, scale, 0, 0);

            Color capColor = color;
            if (!emit)
                capColor = Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f), color);
            spriteBatch.Draw(mushroom.Value, position - Main.screenPosition, capFrame, capColor, rotation, origin, scale, 0, 0);
        }
    }
}
