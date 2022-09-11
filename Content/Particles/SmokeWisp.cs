﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using ParticleEngine;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

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
            lifeMax = (int)(scale * 6 + 12);
            scale *= Main.rand.NextFloat(0.9f, 1.1f);
            frame = Main.rand.Next(5);
            rotation = velocity.ToRotation();
        }

        public override void Update()
        {
            rotation = velocity.ToRotation();

            velocity = Vector2.Lerp(velocity, velocity.RotatedByRandom(0.7f), 0.2f);
            velocity *= 1.03f;

            scale *= Main.rand.NextFloat(0.95f, 1f);

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

            float fade = Utils.GetLerpValue(0, 3 * scale, life, true) * Utils.GetLerpValue(lifeMax, lifeMax - 15 * scale, life, true);
            Vector2 stretch = new Vector2(1.5f, velocity.Length() * 0.1f + 0.5f) * scale;
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, variant, Color.Black * 0.15f * fade * fade, rotation - MathHelper.PiOver2, variant.Size() * new Vector2(0.5f, 0.7f), stretch * 1.4f, 0, 0);
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, variant, color.MultiplyRGBA(lightColor) * fade, rotation - MathHelper.PiOver2, variant.Size() * new Vector2(0.5f, 0.8f), stretch, 0, 0);
        }
    }
}