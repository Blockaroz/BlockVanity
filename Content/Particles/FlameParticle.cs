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

    private float Progress => (float)timeLeft / maxTime;

    public FlameParticle(Color color, Color fadeColor, int timeLeft, Vector2 gravity = default)
    {
        this.color = color;
        this.fadeColor = fadeColor;
        maxTime = timeLeft;
        style = Main.rand.Next(15);
        this.gravity = gravity;
    }

    public void OnSpawn(Particle particle)
    {
        particle.scale *= Main.rand.NextFloat(0.9f, 1.1f);
        rotationVelocity = Math.Sign(particle.velocity.X) * Main.rand.NextFloat() * -0.2f;
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
        Texture2D texture = AllAssets.Textures.Particle[0].Value;
        Texture2D glow = AllAssets.Textures.Glow[0].Value;
        Rectangle frame = texture.Frame(1, 15, 0, style);
        Color drawColor = Color.Lerp(color, fadeColor, Utils.GetLerpValue(0f, 0.4f, Progress, true)) * Utils.GetLerpValue(1f, 0.7f, Progress, true);
        float drawScale = particle.scale * MathF.Sqrt(Utils.GetLerpValue(0f, 2f, timeLeft, true)) * (0.7f + Progress * 0.6f);

        spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), fadeColor * Utils.GetLerpValue(0.5f, 0f, Progress, true) * 0.2f, particle.rotation * 2f, glow.Size() * 0.5f, particle.scale * 0.5f * Utils.GetLerpValue(0f, 1f, timeLeft, true), 0, 0);

        Effect dissolveEffect = AllAssets.Effects.Dissolve.Value;
        dissolveEffect.Parameters["uTexture0"].SetValue(AllAssets.Textures.FireDissolveNoise.Value);
        dissolveEffect.Parameters["uTextureScale"].SetValue(new Vector2(1f + particle.scale * 0.1f + Progress * 0.1f));
        dissolveEffect.Parameters["uFrameCount"].SetValue(15);
        dissolveEffect.Parameters["uProgress"].SetValue(1f - MathF.Sqrt(1f - Progress));
        dissolveEffect.Parameters["uPower"].SetValue(1f + Progress * 60f);
        dissolveEffect.Parameters["uNoiseStrength"].SetValue(1f);
        dissolveEffect.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Draw(texture, particle.position - Main.screenPosition, frame, drawColor, particle.rotation, frame.Size() * new Vector2(0.5f, 0.4f), drawScale * 0.45f, 0, 0);

        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }
}
