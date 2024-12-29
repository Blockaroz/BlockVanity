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
    private Player player;
    private Vector2 oldPos;

    private readonly float Progress => timeLeft / (float)maxTime;

    public FrenziedFlameParticle(int timeLeft, Player hostPlayer)
    {
        maxTime = timeLeft + 2;
        style = Main.rand.Next(6);
        player = hostPlayer;
        if (player != null)
            oldPos = player.position;
    }

    public void OnSpawn(Particle particle)
    {
        particle.rotation = particle.velocity.ToRotation() + Main.rand.NextFloat(-0.2f, 0.2f);
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.96f + Progress * 0.07f;
        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.rotation = Utils.AngleLerp(particle.rotation, particle.velocity.ToRotation(), 0.01f);

        particle.position += particle.velocity;
        if (player != null)
        {
            particle.position += (player.position - oldPos) * (1f - Progress * 0.1f);
            oldPos = player.position;
            if (particle.position.Distance(player.Center) > 220)
                timeLeft += 2;
        }
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[5].Value;
        Rectangle frame = texture.Frame();

        Color drawColor = Color.Lerp(Color.White with { A = 220 }, Color.HotPink with { A = 150 }, Progress) * Utils.GetLerpValue(1f, 0.7f, Progress, true);
        float scaleMod = (0.35f + Utils.GetLerpValue(20, 300, particle.position.Distance(player.Center), true)) * Utils.GetLerpValue(0, 14, timeLeft, true);
        spriteBatch.Draw(texture, particle.position - anchorPosition, frame, drawColor, particle.rotation, frame.Size() * 0.5f, particle.scale * scaleMod, 0, 0);
    }
}
