using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Content.Particles;

public class MagicSmokeParticle : BaseParticle<MagicSmokeParticle>
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Rotation;
    public int MaxTime;
    public int TimeLeft;
    public Color ColorTint;
    public Color ColorGlow;
    public float Scale;
    private int Style;
    private int SpriteEffect;

    public bool LightAffected;

    public static MagicSmokeParticle RequestNew(Vector2 position, Vector2 velocity, float rotation, int lifeTime, Color color, Color glowColor, float scale)
    {
        var smoke = Pool.RequestParticle();
        smoke.Position = position;
        smoke.Velocity = velocity;
        smoke.Rotation = rotation;
        smoke.MaxTime = lifeTime;
        smoke.ColorTint = color;
        smoke.ColorGlow = glowColor;
        smoke.Scale = scale;
        smoke.Style = Main.rand.Next(3);
        smoke.SpriteEffect = Main.rand.Next(2);
        return smoke;
    }

    public override void FetchFromPool()
    {
        base.FetchFromPool();
        Velocity = Vector2.Zero;
        MaxTime = 1;
        TimeLeft = 0;
        LightAffected = false;
    }

    public override void Update(ref ParticleRendererSettings settings)
    {
        Position += Velocity;
        Velocity *= 0.85f;

        TimeLeft++;
        if (TimeLeft > MaxTime)
            ShouldBeRemovedFromRenderer = true;
    }

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        Texture2D texture = Assets.Textures.Particle[1].Value;
        float progress = (float)TimeLeft / MaxTime;
        int frameCount = (int)MathF.Floor(MathF.Sqrt(progress) * 7);
        Rectangle frame = texture.Frame(7, 6, frameCount, Style);
        Rectangle glowFrame = texture.Frame(7, 6, frameCount, Style + 3);
        Color drawColor = Color.Lerp(ColorTint, ColorGlow, Utils.GetLerpValue(0.3f, 0.7f, progress, true)) * Utils.GetLerpValue(1f, 0.9f, progress, true);
        Color glowColor = ColorGlow * Utils.GetLerpValue(1f, 0.5f, progress, true);
        
        if (LightAffected)
        {
            Color lightColor = Lighting.GetColor(Position.ToTileCoordinates());
            drawColor = drawColor.MultiplyRGBA(lightColor);
            glowColor = glowColor.MultiplyRGBA(lightColor);
        }

        spritebatch.Draw(texture, Position + settings.AnchorPosition, frame, drawColor, Rotation + MathHelper.PiOver2, frame.Size() * 0.5f, Scale, (SpriteEffects)SpriteEffect, 0);
        spritebatch.Draw(texture, Position + settings.AnchorPosition, glowFrame, glowColor, Rotation + MathHelper.PiOver2, glowFrame.Size() * 0.5f, Scale, (SpriteEffects)SpriteEffect, 0);
    }
}