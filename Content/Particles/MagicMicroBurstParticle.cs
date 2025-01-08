using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Content.Particles;

public class MagicMicroBurstParticle : BaseParticle
{
    public static ParticlePool<MagicMicroBurstParticle> pool = new ParticlePool<MagicMicroBurstParticle>(500, GetNewParticle<MagicMicroBurstParticle>);

    public Vector2 Position;
    public Vector2 Velocity;
    public float Rotation;
    public int MaxTime;
    public int TimeLeft;
    public Color ColorTint;
    public Color DarkColor;
    public float Scale;
    public int Style;

    public void Prepare(Vector2 position, Vector2 velocity, float rotation, int lifeTime, Color color, Color darkColor, float scale)
    {
        Position = position;
        Velocity = velocity;
        Rotation = rotation;
        MaxTime = lifeTime;
        ColorTint = color;
        DarkColor = darkColor;
        Scale = scale;
        Style = Main.rand.Next(5);
    }

    public override void FetchFromPool()
    {
        base.FetchFromPool();
        Velocity = Vector2.Zero;
        MaxTime = 1;
        TimeLeft = 0;
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
        Texture2D texture = AllAssets.Textures.Particle[6].Value;
        Texture2D glowTexture = AllAssets.Textures.Particle[7].Value;
        Rectangle frame = texture.Frame(1, 7, 0, (int)MathF.Floor((float)TimeLeft / MaxTime * 7));
        float colorFade = MathF.Pow(1f - (float)TimeLeft / MaxTime, 0.1f);

        spritebatch.Draw(glowTexture, Position + settings.AnchorPosition, frame, ColorTint * colorFade, Rotation, frame.Size() * 0.5f, Scale * 0.5f, 0, 0);
    }
}
