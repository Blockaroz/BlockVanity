//using BlockVanity.Core;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using ReLogic.Content;
//using System;
//using Terraria;
//using Terraria.Graphics.Renderers;
//using Terraria.ModLoader;

//namespace BlockVanity.Content.Particles;

//public class GenericFlameParticle : BaseParticle
//{
//    public static ParticlePool<GenericFlameParticle> pool = new ParticlePool<GenericFlameParticle>(1000, GetNewParticle<GenericFlameParticle>);

//    public Vector2 Position;
//    public Vector2 Velocity;
//    public float Rotation;
//    public float Scale;
//    public Color ColorTint;
//    public Color DarkColor;

//    public int MaxTime;
//    public int TimeLeft;
//    private int FlameStyle;
//    private bool Flip;

//    public void Prepare(Vector2 position, Vector2 velocity, int lifeTime, Color color, Color darkColor, float scale)
//    {
//        Position = position;
//        Velocity = velocity;
//        Rotation = Velocity.ToRotation() - Main.rand.NextFloat(-0.1f, 0.1f);
//        Scale = scale;
//        ColorTint = color;
//        DarkColor = darkColor;

//        MaxTime = lifeTime + 2;
//    }

//    public override void FetchFromPool()
//    {
//        base.FetchFromPool();
//        TimeLeft = 0;
//        MaxTime = 1;
//        FlameStyle = Main.rand.Next(3);
//        Flip = Main.rand.NextBool();
//    }

//    public override void Update(ref ParticleRendererSettings settings)
//    {
//        float progress = (float)TimeLeft / MaxTime;

//        Velocity *= 0.9f;
//        Position += Velocity * 0.5f;

//        if (TimeLeft++ > MaxTime)
//            ShouldBeRemovedFromRenderer = true;
//    }

//    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
//    {
//        float progress = (float)TimeLeft / MaxTime;
//        float fadeIn = MathF.Cbrt(Utils.GetLerpValue(0, 40, TimeLeft, true));

//        int frameCounter = (int)Math.Floor(MathF.Sqrt(progress) * 128);
//        Texture2D texture = AllAssets.Textures.GenericFlame[FlameStyle].Value;
//        Rectangle frame = texture.Frame(16, 16, frameCounter % 16, frameCounter / 16);
//        Rectangle glowFrame = texture.Frame(16, 16, frameCounter % 16, frameCounter / 16 + 8);

//        SpriteEffects effect = Flip ? SpriteEffects.FlipHorizontally : 0;

//        float fadeOut = Utils.GetLerpValue(1f, 0.8f, progress, true);
//        Color drawColor = Color.Lerp(ColorTint, DarkColor, Utils.GetLerpValue(0.8f, 0.9f, progress, true));

//        Vector2 stretch = new Vector2(1f, 1f + Velocity.Length() * 0.06f);
//        spritebatch.Draw(texture, Position + settings.AnchorPosition, glowFrame, DarkColor * fadeOut, Rotation + MathHelper.PiOver2, glowFrame.Size() * new Vector2(0.5f, 0.6f), Scale * fadeIn * stretch, effect, 0);
//        spritebatch.Draw(texture, Position + settings.AnchorPosition, frame, drawColor * fadeOut, Rotation + MathHelper.PiOver2, frame.Size() * new Vector2(0.5f, 0.6f), Scale * fadeIn * stretch, effect, 0);           
//    }
//}