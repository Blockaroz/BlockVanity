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
    public class PopHeart : Particle
    {
        private int variant;
        private float life;

        public override void OnSpawn()
        {
            variant = Main.rand.Next(3);
            rotation = Main.rand.NextFloat(-0.2f, 0.2f);
            life = scale * 10;
        }

        public override void Update()
        {
            velocity *= 0.98f;
            rotation *= -1;

            float factor = 0.92f;
            if (data is float newFactor)
                factor = newFactor;
            life *= factor;
            if (life < 2.5f * scale)
                velocity *= 0.7f;

            if (emit)
                Lighting.AddLight(position, color.ToVector3() * 0.12f);

            if (life < 0.05f)
                Active = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            Rectangle frame = texture.Frame(1, 3, 0, variant);

            float t = life / (scale * 10f);
            float bounce = Easing.BackOut(Utils.GetLerpValue(0.1f, 0.9f, t, true));
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, frame, color, rotation, frame.Size() * 0.5f, bounce * scale, 0, 0);

            Color innerColor = Color.Lerp(Color.White, color, 0.7f);
            innerColor.A = 0;
            spriteBatch.Draw(texture.Value, position - Main.screenPosition, frame, innerColor, rotation, frame.Size() * new Vector2(0.5f, 0.55f), bounce * scale * 0.7f, 0, 0);

            Asset<Texture2D> popTexture = TextureAssets.Extra[174];
            float popScale = Utils.GetLerpValue(0.17f, 0.05f, t, true) * 0.22f;
            float popFade = Utils.GetLerpValue(0.04f, 0.13f, t, true);
            spriteBatch.Draw(popTexture.Value, position - Main.screenPosition, null, innerColor * popFade, rotation, popTexture.Size() * 0.5f, popScale * scale, 0, 0);
        }
    }
}
