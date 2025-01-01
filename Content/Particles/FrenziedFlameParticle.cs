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
    private int styleX;
    private int styleY;
    private Player player;
    private Vector2 oldPos;

    private readonly float Progress => timeLeft / (float)maxTime;

    public FrenziedFlameParticle(int timeLeft, Player hostPlayer)
    {
        maxTime = timeLeft + 2;
        styleX = Main.rand.Next(100);
        styleY = Main.rand.Next(100);
        player = hostPlayer;
        if (player != null)
            oldPos = player.position;
    }

    public void OnSpawn(Particle particle)
    {
        particle.rotation = particle.velocity.ToRotation() + Main.rand.NextFloat(-0.1f, 0.1f);
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.97f + Progress * 0.07f;
        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.rotation = Utils.AngleLerp(particle.rotation, particle.velocity.ToRotation(), 0.1f);

        particle.position += particle.velocity;
        if (player != null)
        {
            particle.position += (player.position - oldPos) / 3f * (1f - Progress * 0.1f);
            oldPos = player.position;
            float distance = particle.position.Distance(player.MountedCenter / 3f);
            if (distance > 220)
                timeLeft += 2;

            if (distance > 500)
                particle.active = false;
        }
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[5].Value;
        Rectangle frame = texture.Frame();

        Color drawColor = new Color(styleX / 100f, styleY / 100f, Progress);
        float scaleMod = (0.2f + Progress * 0.2f + Utils.GetLerpValue(20, 300, particle.position.Distance(player.MountedCenter / 3f), true)) * MathF.Sqrt(Utils.GetLerpValue(-5, 5, timeLeft, true));
        spriteBatch.Draw(texture, particle.position - anchorPosition, frame, drawColor, particle.rotation, frame.Size() * 0.5f, particle.scale * scaleMod, 0, 0);
    }
}
