using System;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace BlockVanity.Content.Particles;

public struct ChaosFlameParticle : IParticleData
{
    private int maxTime;
    private int timeLeft;
    private int style;
    private Vector2 gravity;
    private float rotationVelocity;

    private const int FrameCount = 60;
    private readonly float Progress => timeLeft / (float)maxTime;

    public ChaosFlameParticle(int timeLeft, Vector2 gravity = default)
    {
        maxTime = timeLeft;
        this.gravity = gravity;
        style = Main.rand.Next(6);
    }

    public void OnSpawn(Particle particle)
    {
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.98f * (1f - MathF.Pow(Progress, 3f) * 0.5f);
        particle.velocity += gravity;

        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.rotation = Utils.AngleLerp(particle.rotation, 0, 0.003f);
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[4].Value;
        Texture2D bloom = AllAssets.Textures.Glow[0].Value;
        Rectangle frame = texture.Frame(9, 5, (int)MathF.Floor(MathF.Pow(Progress, 1.3f) * 9), style);

        Color baseColor = Color.White;
        Vector2 drawScale = new Vector2(1.1f - Progress * 0.2f) * particle.scale * Utils.GetLerpValue(-2f, 3f, timeLeft, true);
        spriteBatch.Draw(texture, particle.position - anchorPosition, frame, baseColor, particle.rotation, frame.Size() * 0.5f, drawScale, 0, 0);    
    }
}
