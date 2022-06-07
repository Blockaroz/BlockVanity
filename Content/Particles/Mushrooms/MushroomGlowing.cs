﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles.Mushrooms
{
    public class MushroomGlowing : Particle
    {
        private int type; 
        private int time;

        public override void OnSpawn()
        {
            type = Main.rand.Next(3);
            rotation = Main.rand.NextFloat(-0.2f, 0.2f); 
            position.Y += Main.rand.Next(3, 6);
        }

        public override void Update()
        {
            time++;
            if (time > 100)
                Active = false;

            scale *= 0.999f;

            float strength = Easing.CircularOut(Utils.GetLerpValue(40, 80, time, true)) * scale;
            Lighting.AddLight(position - Vector2.UnitY * 3f, Color.Blue.ToVector3() * 0.5f * strength);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> mushroom = ModContent.Request<Texture2D>(Texture);
            Rectangle frame = mushroom.Frame(4, 1, type);
            Vector2 origin = frame.Size() * new Vector2(0.5f, 1f); 
            float growScale = Easing.BackOut(Utils.GetLerpValue(0, 28, time, true)) * Easing.CircularOut(Utils.GetLerpValue(45, 100, time, true)) * scale;

            Color light = Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f), color);
            spriteBatch.Draw(mushroom.Value, position - Main.screenPosition, frame, light, rotation, origin, growScale, 0, 0);
        }
    }
}
