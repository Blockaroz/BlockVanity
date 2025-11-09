using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Content.Particles;

public class PixelSpark : BaseParticle
{
    public static ParticlePool<PixelSpark> Pool { get; } = new ParticlePool<PixelSpark>(1000, GetNewParticle<PixelSpark>);

    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Gravity;
    public float Mass;
    public Color ColorTint;
    public Color ColorGlow;
    public float Scale;
    public bool Collide;
    private Vector2 EndPosition;

    public static PixelSpark RequestNew(Vector2 position, Vector2 velocity, Vector2 gravity, Color color, Color glowColor, float mass, bool collide = true)
    {
        var spark = Pool.RequestParticle();
        spark.Position = position;
        spark.EndPosition = position;
        spark.Velocity = velocity;
        spark.Gravity = gravity;
        spark.ColorTint = color;
        spark.ColorGlow = glowColor;
        spark.Mass = mass;
        spark.Collide = collide;
        spark.Scale = Main.rand.NextFloat(0.9f, 1.1f);
        return spark;
    }

    public override void FetchFromPool()
    {
        base.FetchFromPool();
        Velocity = Vector2.Zero;
    }

    public override void Update(ref ParticleRendererSettings settings)
    {
        Position += Velocity;
        Velocity += Gravity * (0.5f - Scale * 0.5f);
        if (Velocity.Length() > 24f)
            Velocity = Vector2.Lerp(Velocity, Velocity.SafeNormalize(Vector2.Zero) * 24f, 0.4f);

        float lerpFactor = 0.2f;
        if (Collide)
        {
            Vector2 bounce = Collision.TileCollision(Position - new Vector2(2, 3), Velocity, 4, 2);
            if (Math.Abs(bounce.X - Velocity.X) > 0)
            {
                Velocity.X *= -0.3f;
                lerpFactor = 0.5f;
            }

            if (Math.Abs(bounce.Y - Velocity.Y) > 0)
            {
                Scale *= 0.95f;
                Velocity.Y *= -Main.rand.NextFloat(0.6f, 0.9f);
                Velocity.X *= 0.6f;
                lerpFactor = 0.5f;
            }

            if (Collision.SolidCollision(Position - Vector2.One, 2, 2))
                Scale *= 0.8f;
        }

        if (Scale < 0.1f)
        {
            Scale *= 0.9f + Math.Clamp(Mass, 0f, 1f) * 0.01f;
            lerpFactor = 0.5f;

            if (Scale < 0.01f)
                ShouldBeRemovedFromRenderer = true;
        }
        else
            Scale *= 0.4f + Math.Clamp(Mass * 0.5f, 0f, 0.5f);

        EndPosition = Vector2.Lerp(EndPosition, Position - Velocity, lerpFactor);
    }

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        Texture2D texture = Assets.Textures.Particle[0].Value;
        Texture2D glow = Assets.Textures.Glow[0].Value;

        float rotation = MathF.Round(Position.AngleFrom(EndPosition) / MathHelper.TwoPi * 8f) / 8f * MathHelper.TwoPi;
        float fadeOut = Utils.GetLerpValue(0f, 0.05f, Scale, true);

        Vector2 particlePos = new Vector2((int)Math.Round(Position.X / 2) * 2, (int)Math.Round(Position.Y / 2) * 2) + settings.AnchorPosition;
        Vector2 unRotatedVel = Velocity.RotatedBy(-rotation);
        Vector2 stretch = new Vector2(1.1f + Position.Distance(EndPosition) * 0.15f, 1f) * (1f + Mass * 0.01f);

        spritebatch.Draw(texture, particlePos, texture.Frame(), ColorTint * fadeOut, rotation, texture.Size() * 0.5f, stretch, 0, 0);
        spritebatch.Draw(glow, particlePos, glow.Frame(), ColorGlow with { A = 0 } * fadeOut, rotation, glow.Size() * 0.5f, new Vector2(0.1f, 0.15f) * stretch, 0, 0);    
    }
}
