using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.Map;

namespace BlockVanity.Content.Particles;

public class PhysicalSparkParticle : BaseParticle
{
    public static ParticlePool<PhysicalSparkParticle> pool = new ParticlePool<PhysicalSparkParticle>(1000, GetNewParticle<PhysicalSparkParticle>);

    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Gravity;
    public float Mass;
    public Color ColorTint;
    public Color ColorGlow;
    public float Scale;
    public bool Collide;
    private Vector2 EndPosition;

    public void Prepare(Vector2 position, Vector2 velocity, Vector2 gravity, Color color, Color glowColor, float mass, bool collide = true)
    {
        Position = position;
        EndPosition = position;
        Velocity = velocity;
        Gravity = gravity;
        ColorTint = color;
        ColorGlow = glowColor;
        Mass = mass;
        Collide = collide;
        Scale = Main.rand.NextFloat(0.8f, 1.3f);
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

        float lerpFactor = 0.1f;
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
                Velocity.Y *= -Main.rand.NextFloat(0.3f, 0.6f);
                Velocity.X *= 0.6f;
                lerpFactor = 0.5f;
            }

            if (Collision.SolidCollision(Position - Vector2.One, 2, 2))
                Scale *= 0.8f;
        }

        if (Scale < 0.1f)
        {
            Scale *= 0.9f + Math.Clamp(Mass, 0f, 1f) * 0.06f;
            lerpFactor = 0.7f;

            if (Scale < 0.01f)
                ShouldBeRemovedFromRenderer = true;
        }
        else
            Scale *= 0.5f + Math.Clamp(Mass * 0.2f, 0f, 0.4f);

        EndPosition = Vector2.Lerp(EndPosition, Position - Velocity, lerpFactor);
    }

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        Texture2D texture = AllAssets.Textures.Glow[1].Value;
        Texture2D tailTexture = AllAssets.Textures.Particle[2].Value;

        float rotation = Position.AngleFrom(EndPosition) + MathHelper.PiOver2;
        float thickness = (Mass * 0.2f + 2f);
        float fadeOut = Utils.GetLerpValue(0f, 0.07f, Scale, true);

        Vector2 glowStretch = new Vector2(thickness * 0.3f, (Position.Distance(EndPosition) + 2) / tailTexture.Height) * fadeOut;
        Vector2 midStretch = new Vector2(thickness * 0.08f, Position.Distance(EndPosition) / tailTexture.Height) * fadeOut;

        spritebatch.Draw(tailTexture, Position + settings.AnchorPosition, tailTexture.Frame(), ColorGlow, rotation, tailTexture.Size() * new Vector2(0.5f, 0.25f / tailTexture.Height), glowStretch, 0, 0);
        spritebatch.Draw(texture, Position + settings.AnchorPosition, texture.Frame(), ColorGlow, rotation, texture.Size() * 0.5f, 0.07f * thickness * fadeOut, 0, 0);
        
        spritebatch.Draw(tailTexture, Position + settings.AnchorPosition, tailTexture.Frame(), ColorTint, rotation, tailTexture.Size() * new Vector2(0.5f, 0.25f / tailTexture.Height), midStretch, 0, 0);
        spritebatch.Draw(texture, Position + settings.AnchorPosition, texture.Frame(), ColorTint, rotation, texture.Size() * 0.5f, 0.02f * thickness * fadeOut, 0, 0);
    }
}
