using System;
using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace BlockVanity.Content.Particles.SpecialParticles;

public struct FrenziedFlameParticle : IParticleData
{
    private int maxTime;
    private int timeLeft;
    private int frameStyle;
    private int styleX;
    private int styleY;
    private Player player;
    private Vector2 oldPos;
    private float strayPercent;

    private readonly float Progress => timeLeft / (float)maxTime;

    public FrenziedFlameParticle(int timeLeft, float strayPercent, Player player)
    {
        maxTime = timeLeft + 2;
        frameStyle = Main.rand.Next(5);
        styleX = Main.rand.Next(100);
        styleY = Main.rand.Next(100);
        this.player = player;
        this.strayPercent = strayPercent;
        if (this.player != null)
            oldPos = this.player.position;
    }

    public void OnSpawn(Particle particle)
    {
        particle.rotation = particle.velocity.ToRotation();
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.91f;
        if (timeLeft++ > maxTime)
            particle.active = false;

        particle.position += particle.velocity;

        if (player != null)
        {
            Vector2 difference = (player.position - oldPos) / 3f;
            if (difference.Length() > 200)
                particle.position += difference;
            else
                particle.position += difference * (1f - Progress * strayPercent);

            oldPos = player.position;
            float distance = particle.position.Distance(player.MountedCenter / 3f);
            if (distance > 250)
                timeLeft++;

            if (distance > 360)
                particle.active = false;
        }
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[5].Value;
        Rectangle frame = texture.Frame(1, 5, 0, frameStyle);

        float fadeIn = MathF.Cbrt(Utils.GetLerpValue(-5, 25, timeLeft, true));
        SpriteEffects effect = (SpriteEffects)(styleX ^ styleY);
        Color drawColor = new Color(styleX / 100f, styleY / 100f, Progress * (fadeIn * 0.5f + 0.5f));
        float scaleMod = (0.2f + Progress * 0.2f + Utils.GetLerpValue(20, 700, particle.position.Distance(player.MountedCenter / 3f), true)) * fadeIn;
        spriteBatch.Draw(texture, particle.position - anchorPosition, frame, drawColor, particle.rotation, frame.Size() * 0.5f, particle.scale * scaleMod, effect, 0);
    }
}
