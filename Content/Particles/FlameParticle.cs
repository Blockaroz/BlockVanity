using System;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using static tModPorter.ProgressUpdate;

namespace BlockVanity.Content.Particles;

public struct FlameParticle : IShaderParticleData
{
    private int maxTime;
    private int timeLeft;
    private Color color;
    private Color fadeColor;
    private int style;
    private Vector2 gravity;
    private float rotationVelocity;
    private float fadeTime;

    private readonly float Progress => timeLeft / (maxTime - 1f);

    public FlameParticle(Color color, Color fadeColor, int timeLeft, Vector2 gravity = default, float fadeTime = 0.25f)
    {
        this.color = color;
        this.fadeColor = fadeColor;
        maxTime = timeLeft;
        style = Main.rand.Next(15);
        this.fadeTime = fadeTime;
        this.gravity = gravity;
    }

    public void OnSpawn(Particle particle)
    {
        particle.scale *= Main.rand.NextFloat(0.8f, 1.2f);
        particle.rotation = particle.velocity.ToRotation() - MathHelper.PiOver2;
        rotationVelocity = Math.Sign(particle.velocity.X) * Main.rand.NextFloat() * -0.15f;
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.97f - Progress * 0.2f;
        particle.velocity += gravity;

        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.rotation += (1f - MathF.Cbrt(Progress)) * rotationVelocity;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch)
    {
        Texture2D texture = AllAssets.Textures.Particle[1].Value;
        Texture2D glow = AllAssets.Textures.Glow[0].Value;
        Rectangle frame = texture.Frame(1, 15, 0, style);
        float drawScale = particle.scale * MathF.Sqrt(Utils.GetLerpValue(-2f, 2f, timeLeft, true)) * (0.7f + MathF.Pow(Progress, 2f));

        Color drawColor = Color.Lerp(color, fadeColor, Utils.GetLerpValue(0.1f - fadeTime * 0.2f, 0.1f + fadeTime, Progress, true));
        Color glowColor = fadeColor * MathF.Pow(Utils.GetLerpValue(0.7f, 0f, Progress, true), 2f) * 0.12f;
        spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), glowColor, particle.rotation, glow.Size() * 0.5f, drawScale * 0.66f, 0, 0);

        Effect dissolveEffect = AllAssets.Effects.Dissolve.Value;
        dissolveEffect.Parameters["uTexture0"].SetValue(AllAssets.Textures.FireDissolveNoise.Value);
        dissolveEffect.Parameters["uTextureScale"].SetValue(new Vector2(0.65f * (0.7f + particle.scale * 0.3f) + Progress * 0.05f));
        dissolveEffect.Parameters["uFrameCount"].SetValue(15);
        dissolveEffect.Parameters["uSourceRect"].SetValue(new Vector4(frame.X, frame.Y, frame.Width, frame.Height));
        dissolveEffect.Parameters["uProgress"].SetValue(Progress / 3f);
        dissolveEffect.Parameters["uPower"].SetValue(1f + Progress * 60f);
        dissolveEffect.Parameters["uNoiseStrength"].SetValue(1f);
        dissolveEffect.Parameters["uRotation"].SetValue(-particle.rotation * Math.Sign(rotationVelocity) * 0.9f + gravity.ToRotation());
        dissolveEffect.CurrentTechnique.Passes[0].Apply();

        SpriteEffects flip = rotationVelocity > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        spriteBatch.Draw(texture, particle.position - Main.screenPosition, frame, drawColor, particle.rotation, frame.Size() * 0.5f, drawScale * 0.45f, flip, 0);

        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }
}
