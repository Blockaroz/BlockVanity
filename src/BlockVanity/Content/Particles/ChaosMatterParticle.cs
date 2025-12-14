using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles;

public class ChaosMatterParticle : BaseParticle<ChaosMatterParticle>
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

    public static ChaosMatterParticle RequestNew(Vector2 position, Vector2 velocity, Vector2 gravity, int lifeTime, float rotation, float scale)
    {
        var chaos = Pool.RequestParticle();
        chaos.Position = position;
        chaos.Velocity = velocity;
        chaos.Gravity = gravity;
        chaos.Rotation = rotation;
        chaos.Scale = scale;
        chaos.MaxTime = lifeTime + 2;
        chaos.Gravity = gravity;
        return chaos;
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
            ShouldBeRemovedFromRenderer = true;

        Position += Velocity;
    }

    public static LazyAsset<Texture2D> ChaosMatterParticleTexture = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Particles/ChaosMatterParticle");

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        float progress = (float)TimeLeft / MaxTime;

        Texture2D texture = ChaosMatterParticleTexture.Value;
        Rectangle frame = texture.Frame(9, 5, (int)MathF.Floor(progress * 9), Style);

        Vector2 roundedPosition = (Position + settings.AnchorPosition).Floor();
        spritebatch.Draw(texture, roundedPosition, frame, Color.White, Rotation, frame.Size() * 0.5f, Scale, (SpriteEffects)FlipSprite.ToInt(), 0);
    }
}