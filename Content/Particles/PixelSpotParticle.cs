using System;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles;

public class PixelSpotParticle : BaseParticle
{
    public static ParticlePool<PixelSpotParticle> pool = new ParticlePool<PixelSpotParticle>(500, GetNewParticle<PixelSpotParticle>);
    
    public Vector2 Position;
    public Vector2 Velocity;
    public float Rotation;
    public int MaxTime;
    public int TimeLeft;
    public int FadeIn;
    public Color ColorTint;
    public Color DarkColor;
    public float Scale;

    public void Prepare(Vector2 position, Vector2 velocity, int lifeTime, int fadeIn, Color color, Color darkColor, float scale)
    {
        Position = position;
        Velocity = velocity;
        Rotation = MathF.Round(Velocity.ToRotation() / MathHelper.TwoPi * 8f) / 8f * MathHelper.TwoPi;
        MaxTime = lifeTime;
        FadeIn = fadeIn;
        ColorTint = color;
        DarkColor = darkColor;
        Scale = scale;
    }

    public override void FetchFromPool()
    {
        base.FetchFromPool();
        MaxTime = 1;
        TimeLeft = 0;
        Velocity = Vector2.Zero;
    }

    public override void Update(ref ParticleRendererSettings settings)
    {
        float progress = (float)TimeLeft / MaxTime;

        Velocity += Main.rand.NextVector2Circular(1, 1) * 0.15f * progress;
        Velocity *= 0.97f;
        Velocity.Y -= Main.rand.NextFloat(-0.5f, 2f) * 0.01f;
        Rotation = MathF.Round(Velocity.ToRotation() / MathHelper.TwoPi * 8f) / 8f * MathHelper.TwoPi;

        Lighting.AddLight(Position, DarkColor.ToVector3() * 0.2f * (1f - progress));

        if (TimeLeft++ > MaxTime || Scale < 0.01f)
            ShouldBeRemovedFromRenderer = true;

        Scale = MathHelper.Lerp(Scale, 1f, 0.002f);
        Position += Velocity;
    }

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        Texture2D texture = AllAssets.Textures.Particle[0].Value;
        Texture2D glow = AllAssets.Textures.Glow[2].Value;

        float progress = (float)TimeLeft / MaxTime;
        float fadeIn = FadeIn > 0f ? Utils.GetLerpValue(0f, FadeIn, TimeLeft, true) : 1f;

        Color glowColor = DarkColor * MathF.Sqrt(1f - progress) * fadeIn;
        Color drawColor = Color.Lerp(ColorTint, glowColor, progress) * fadeIn * fadeIn;

        Vector2 particlePos = new Vector2((int)Math.Round(Position.X), (int)Math.Round(Position.Y)) + settings.AnchorPosition;
        Vector2 unRotatedVel = Velocity.RotatedBy(-Rotation);
        float xS = Math.Abs(unRotatedVel.X) * 3f + Scale;
        float yS = Math.Abs(unRotatedVel.Y) * 3f + Scale;
        Vector2 stretch = new Vector2(Math.Max(xS - yS * 0.45f, 2f), Math.Max(yS - xS * 0.1f, 2f)) * 0.5f;

        spritebatch.Draw(texture, particlePos, texture.Frame(), drawColor, Rotation, texture.Size() * 0.5f, stretch, 0, 0);
        spritebatch.Draw(glow, particlePos, glow.Frame(), glowColor with { A = 0 }, Rotation, glow.Size() * 0.5f, stretch, 0, 0);
    }
}
