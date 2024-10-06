using System;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace BlockVanity.Content.Particles;

public struct FlameParticle : IParticleData
{
    private int maxTime;
    private int timeLeft;
    private Color color;
    private Color fadeColor;
    private int style;
    private Vector2 gravity;
    private float rotationVelocity;

    private const int FrameCount = 60;
    private readonly float Progress => timeLeft / (float)maxTime;

    public FlameParticle(Color color, Color fadeColor, int timeLeft, Vector2 gravity = default)
    {
        this.color = color;
        this.fadeColor = fadeColor;
        maxTime = timeLeft;
        this.gravity = gravity;
        rotationVelocity = Main.rand.NextFloat(-0.03f, 0.03f);
        style = Main.rand.Next(6);
    }

    public void OnSpawn(Particle particle)
    {
    }

    public void Update(Particle particle)
    { 
        particle.velocity *= 0.98f;
        particle.velocity += gravity;

        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.rotation += (1f - MathF.Sqrt(Progress)) * rotationVelocity;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[1].Value;
        Texture2D bloom = AllAssets.Textures.Particle[2].Value;
        Rectangle frame = texture.Frame(FrameCount / 3, 6 * 3, (int)Math.Floor(Progress * FrameCount) % 20, 6 * (int)Math.Floor(Progress * 3) + style);

        Color baseColor = Color.Lerp(color, fadeColor, Utils.GetLerpValue(0.25f, 0.45f, Progress, true)) * (1f - Progress);
        Color bloomColor = fadeColor * (1f - Progress);

        Vector2 drawScale = new Vector2(1f - Progress * 0.1f, 1f + Progress * 0.1f) * particle.scale * 0.8f * Utils.GetLerpValue(-5f, 4f, timeLeft, true);
        spriteBatch.Draw(bloom, particle.position - anchorPosition, frame, bloomColor, particle.rotation, frame.Size() * 0.5f, drawScale, 0, 0);        
        spriteBatch.Draw(texture, particle.position - anchorPosition, frame, baseColor, particle.rotation, frame.Size() * 0.5f, drawScale, 0, 0);    
    }
}
