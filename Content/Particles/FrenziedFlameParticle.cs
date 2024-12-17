using System;
using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace BlockVanity.Content.Particles;

public struct FrenziedFlameParticle : IParticleData
{
    private int maxTime;
    private int timeLeft;
    private int style;

    private readonly float Progress => timeLeft / (float)maxTime;

    public FrenziedFlameParticle(int timeLeft)
    {
        maxTime = timeLeft + 2;
        style = Main.rand.Next(6);
    }

    public void OnSpawn(Particle particle)
    {
        particle.rotation = particle.velocity.ToRotation();
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.98f;
        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.rotation = Utils.AngleLerp(particle.rotation, particle.velocity.ToRotation(), 0.02f);

        particle.position += particle.velocity;
    }

    private const int FrameCount = 60;

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[1].Value;
        Texture2D bloom = AllAssets.Textures.Particle[2].Value;
        Rectangle frame = texture.Frame(FrameCount / 3, 6 * 3, (int)Math.Floor(Progress * FrameCount) % 20, 6 * (int)Math.Floor(Progress * 3) + style);

        Color baseColor = Color.Lerp(Color.Goldenrod, Color.Red, Utils.GetLerpValue(0.3f, 0.7f, Progress, true)) with { A = 150 };
        Color bloomColor = new Color(90, 0, 0, 100);

        Vector2 drawScale = new Vector2(1f - Progress * 0.2f, 0.6f + Progress * 0.5f) * particle.scale * 0.7f * Utils.GetLerpValue(-25f, 20f, timeLeft, true);
        spriteBatch.Draw(texture, particle.position - anchorPosition, frame, baseColor, particle.rotation, frame.Size() * 0.5f, drawScale, 0, 0);
        spriteBatch.Draw(bloom, particle.position - anchorPosition, frame, bloomColor, particle.rotation, frame.Size() * 0.5f, drawScale, 0, 0);

    }
}
