using System;
using BlockVanity.Common.Graphics;
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
    private int randomEffect;

    private readonly float Progress => timeLeft / (float)maxTime;

    public ChaosFlameParticle(int timeLeft, Vector2 gravity = default)
    {
        maxTime = timeLeft + 2;
        this.gravity = gravity;
        style = Main.rand.Next(6);
        randomEffect = Main.rand.NextBool() ? 0 : 1;
    }

    public void OnSpawn(Particle particle)
    {
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 1f - MathF.Pow(Progress, 3f) * 0.3f;
        particle.velocity += gravity;
        gravity *= 0.98f;

        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.position += particle.velocity;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[4].Value;
        Texture2D bloom = AllAssets.Textures.Glow[0].Value;
        Rectangle frame = texture.Frame(9, 5, (int)MathF.Floor(Progress * 9), style);

        Color baseColor = Color.White;
        spriteBatch.Draw(texture, particle.position - anchorPosition, frame, baseColor, particle.rotation, frame.Size() * 0.5f, particle.scale, (SpriteEffects)randomEffect, 0);    
    }
}
