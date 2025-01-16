using System;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Content.Particles.SpecialParticles;

public class FrenziedFlameParticle : BaseParticle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Rotation;
    public float Scale;

    public int MaxTime;
    public int TimeLeft;
    public float StrayPercent;
    public Player HostPlayer;

    private int FlameStyle;
    private int NoiseStyleX;
    private int NoiseStyleY;
    private Vector2 OldPos;

    public void Prepare(Vector2 position, Vector2 velocity, float scale, int lifeTime, float strayPercent = 1f, Player player = null)
    {
        Position = position;
        Velocity = velocity;
        Rotation = Velocity.ToRotation() - Main.rand.NextFloat(-1f, 1f);
        Scale = scale;

        MaxTime = lifeTime + 2;
        StrayPercent = strayPercent;
        HostPlayer = player;
        if (HostPlayer != null)
        {
            OldPos = HostPlayer.position;
        }
    }

    public override void FetchFromPool()
    {
        base.FetchFromPool();
        TimeLeft = 0;
        MaxTime = 1;
        FlameStyle = Main.rand.Next(2);
        NoiseStyleX = Main.rand.Next(100);
        NoiseStyleY = Main.rand.Next(100);
    }

    public override void Update(ref ParticleRendererSettings settings)
    {
        float progress = (float)TimeLeft / MaxTime;

        Velocity *= 0.95f;
        Position += Velocity;

        if (HostPlayer != null)
        {
            Vector2 difference = (HostPlayer.position - OldPos) / 3f;
            if (difference.Length() > 200)
            {
                Position += difference;
            }
            else
            {
                Position += difference * (1f - progress * StrayPercent);
            }

            OldPos = HostPlayer.position;
            float distance = Position.Distance(HostPlayer.MountedCenter / 3f);
            if (distance > 250)
            {
                TimeLeft++;
            }

            if (distance > 360)
            {
                ShouldBeRemovedFromRenderer = true;
            }
        }

        if (TimeLeft++ > MaxTime)
        {
            ShouldBeRemovedFromRenderer = true;
        }
    }

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        float progress = (float)TimeLeft / MaxTime;

        Texture2D texture = AllAssets.Textures.Particle[5].Value;
        Rectangle frame = texture.Frame(1, 5, 0, FlameStyle);

        float fadeIn = MathF.Sin(Utils.GetLerpValue(0, 50, TimeLeft, true) * MathHelper.PiOver2);
        SpriteEffects effect = (NoiseStyleX > 50 ? SpriteEffects.FlipHorizontally : 0) | (NoiseStyleY > 60 ? SpriteEffects.FlipVertically : 0);

        Color drawColor = new Color(NoiseStyleX / 100f, NoiseStyleY / 100f, progress * (fadeIn * 0.5f + 0.5f));
        float scaleMod = (0.3f + MathF.Pow(progress, 2) * 0.4f + Utils.GetLerpValue(20, 700, Position.Distance(HostPlayer.MountedCenter / 3f), true)) * fadeIn;
        spritebatch.Draw(texture, Position + settings.AnchorPosition, frame, drawColor, Rotation - MathHelper.PiOver2, frame.Size() * 0.5f, Scale * scaleMod, effect, 0);
    }
}
