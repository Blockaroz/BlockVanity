using System;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles;

public struct ObsoleteShaderFlameParticle : IShaderParticleData
{
    private int maxTime;
    private int timeLeft;
    private Color color;
    private Color fadeColor;
    private int style;
    private Vector2 gravity;
    private float rotationVelocity;
    private float fadeTime;

    private readonly float Progress => timeLeft / (float)maxTime;

    public ObsoleteShaderFlameParticle(Color color, Color fadeColor, int timeLeft, Vector2 gravity = default, float fadeTime = 0.25f)
    {
        this.color = color;
        this.fadeColor = fadeColor;
        maxTime = timeLeft;
        style = Main.rand.Next(6);
        this.fadeTime = fadeTime;
        this.gravity = gravity;
    }

    public void OnSpawn(Particle particle)
    {
        //particle.scale *= Main.rand.NextFloat(0.6f, 1.2f);
        //particle.rotation = particle.velocity.ToRotation() - MathHelper.PiOver2;
        //rotationVelocity = Main.rand.NextFloat(-0.15f, 0.15f);
    }

    public void Update(Particle particle)
    {
        style = 0;
        particle.velocity = Vector2.Zero;
        particle.rotation = 0f;

        //particle.velocity *= 0.98f - Progress * 0.1f;
        //particle.velocity += gravity;

        if (timeLeft++ > maxTime)
        {
            maxTime = 60;
            timeLeft = 0;
            particle.active = false;
        }

        //particle.rotation += (1f - MathF.Cbrt(Progress)) * rotationVelocity;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch)
    {
        Texture2D texture = ModContent.Request<Texture2D>("BlockVanity/Assets/Textures/Particles/Particle_1_Test").Value;

        Rectangle frame = texture.Frame(1, 6, 0, style);
        float drawScale = 1f;//particle.scale * MathF.Sqrt(Utils.GetLerpValue(-1f, 4f, timeLeft, true));

        Color drawColor = Color.Lerp(color, fadeColor, Utils.GetLerpValue(0.2f - fadeTime * 0.2f, 0.3f + fadeTime, Progress, true));
        Color glowColor = fadeColor * MathF.Pow(Utils.GetLerpValue(0.7f, 0f, Progress, true), 2f) * 0.12f;
        //spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), glowColor, particle.rotation, glow.Size()  sd* 0.5f, drawScale * 0.8f, 0, 0);        

        Effect dissolveEffect = AllAssets.Effects.Dissolve.Value;
        dissolveEffect.Parameters["uTexture0"].SetValue(AllAssets.Textures.FireDissolveNoise[0].Value);
        dissolveEffect.Parameters["uTexture1"].SetValue(AllAssets.Textures.FireDissolveNoise[1].Value);
        dissolveEffect.Parameters["uTextureScale"].SetValue(new Vector2(0.7f + Progress * 0.05f));
        dissolveEffect.Parameters["uFrameCount"].SetValue(6);
        dissolveEffect.Parameters["uSourceRect"].SetValue(new Vector4(frame.X, frame.Y, frame.Width, frame.Height));
        dissolveEffect.Parameters["uPower"].SetValue(0.1f + Progress * 16f);
        dissolveEffect.Parameters["uProgress"].SetValue(Progress);
        dissolveEffect.Parameters["uNoiseStrength"].SetValue(1f);
        //dissolveEffect.CurrentTechnique.Passes[0].Apply();
        SpriteEffects flip = rotationVelocity > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        //spriteBatch.Draw(texture, particle.position - Main.screenPosition, frame, drawColor, particle.rotation, frame.Size() * 0.5f, drawScale, flip, 0);

        int texSize = 96;
        float styleCount = 6;
        float frameCountForSheet = 60;
        int framesOffset = 40;
        float framesOnScreen = 20;

        spriteBatch.Draw(TextureAssets.BlackTile.Value, Vector2.UnitY * 80, new Rectangle(0, 0, (int)(frameCountForSheet * texSize), (int)(texSize * styleCount)), Color.Black, particle.rotation, Vector2.Zero, drawScale, 0, 0);

        for (int i = 0; i < framesOnScreen; i++)
        {
            float fakeProgress = (i + framesOffset) / (frameCountForSheet);

            dissolveEffect.Parameters["uTextureScale"].SetValue(new Vector2(0.4f + fakeProgress * 0.7f) * new Vector2(1.5f, 1f));
            dissolveEffect.Parameters["uPower"].SetValue(0.2f + MathF.Pow(fakeProgress, 2f) * 30f);
            dissolveEffect.Parameters["uProgress"].SetValue(fakeProgress);

            for (int j = 0; j < styleCount; j++)
            {
                frame = texture.Frame(1, 6, 0, j);
                dissolveEffect.Parameters["uSourceRect"].SetValue(new Vector4(frame.X, frame.Y, frame.Width, frame.Height));
                dissolveEffect.CurrentTechnique.Passes[0].Apply();

                Vector2 offset = new Vector2(texSize * i, texSize * j);

                spriteBatch.Draw(texture, Vector2.UnitY * 80 + offset + new Vector2(texSize / 2), frame, Color.White, particle.rotation, frame.Size() * 0.5f, drawScale, 0, 0);
            }
        }

        //drawScale = MathF.Sqrt(Utils.GetLerpValue(-1f, 4f, timeLeft, true)) * (0.7f + MathF.Pow(Progress, 2f));

        for (int i = 0; i < styleCount; i++)
        {
            dissolveEffect.Parameters["uTextureScale"].SetValue(new Vector2(0.4f + Progress * 0.7f) * new Vector2(1.5f, 1f));
            dissolveEffect.Parameters["uPower"].SetValue(0.2f + MathF.Pow(Progress, 2f) * 30f);
            dissolveEffect.Parameters["uProgress"].SetValue(Progress);
            frame = texture.Frame(1, 6, 0, i);
            dissolveEffect.Parameters["uSourceRect"].SetValue(new Vector4(frame.X, frame.Y, frame.Width, frame.Height));
            dissolveEffect.CurrentTechnique.Passes[0].Apply();


            spriteBatch.Draw(texture, Vector2.UnitY * (styleCount * 96 + 80) + new Vector2(48) + Vector2.UnitX * i * 96, frame, Color.White, particle.rotation, frame.Size() * 0.5f, drawScale, 0, 0);
        }

        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }
}
