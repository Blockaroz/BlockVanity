using System;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Content.Particles.SpecialParticles;

public class ChaosMatterParticle : BaseParticle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Gravity;
    public float Rotation;
    public float Scale;

    public int MaxTime;
    public int TimeLeft;

    private int Style;
    private bool FlipSprite;

    public void Prepare(Vector2 position, Vector2 velocity, Vector2 gravity, int lifeTime, float rotation, float scale)
    {
        Position = position;
        Velocity = velocity;
        Gravity = gravity;
        Rotation = rotation;
        Scale = scale;
        MaxTime = lifeTime + 2;
        Gravity = gravity;
    }

    public override void FetchFromPool()
    {
        base.FetchFromPool();
        TimeLeft = 0;
        MaxTime = 1;
        Style = Main.rand.Next(6);
        FlipSprite = Main.rand.NextBool();
    }

    public override void Update(ref ParticleRendererSettings settings)
    {
        float progress = (float)TimeLeft / MaxTime;

        Velocity *= 1f - MathF.Pow(progress, 3f) * 0.2f;
        Velocity += Gravity;
        Gravity *= 0.98f;

        if (TimeLeft++ > MaxTime)
        {
            ShouldBeRemovedFromRenderer = true;
        }

        Position += Velocity;
    }

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        float progress = (float)TimeLeft / MaxTime;

        Texture2D texture = AllAssets.Textures.Particle[4].Value;
        Rectangle frame = texture.Frame(9, 5, (int)MathF.Floor(progress * 9), Style);

        Vector2 roundedPosition = (Position + settings.AnchorPosition).Floor();
        spritebatch.Draw(texture, roundedPosition, frame, Color.White, Rotation, frame.Size() * 0.5f, Scale, (SpriteEffects)(FlipSprite.ToInt()), 0);
    }
}
