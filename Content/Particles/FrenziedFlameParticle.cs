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
    private int randomEffect;

    private readonly float Progress => timeLeft / (float)maxTime;

    public FrenziedFlameParticle(int timeLeft)
    {
        maxTime = timeLeft + 2;
        style = Main.rand.Next(6);
        randomEffect = Main.rand.Next(3);
    }

    public void OnSpawn(Particle particle)
    {
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 1f - MathF.Pow(Progress, 3f) * 0.3f;

        if (timeLeft++ > maxTime)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
    }
}
