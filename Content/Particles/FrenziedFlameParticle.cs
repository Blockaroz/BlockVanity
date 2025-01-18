using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles;

public class FrenziedFlameParticle : BaseParticle, ILoadable
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
    private int StartFrame;
    private int Pinkness; 
    private Vector2 OldPos;

    public void Prepare(Vector2 position, Vector2 velocity, float scale, int lifeTime, float strayPercent = 1f, Player player = null)
    {
        Position = position;
        Velocity = velocity;
        Rotation = Velocity.ToRotation() - Main.rand.NextFloat(-0.1f, 0.1f);
        Scale = scale;

        MaxTime = lifeTime + 2;
        StrayPercent = strayPercent;
        HostPlayer = player;

        if (HostPlayer != null)
            OldPos = HostPlayer.position;
    }

    public override void FetchFromPool()
    {
        base.FetchFromPool();
        TimeLeft = 0;
        MaxTime = 1;
        FlameStyle = Main.rand.Next(3);
        StartFrame = Main.rand.Next(20, 50);
        Pinkness = Main.rand.Next(100);
    }

    public override void Update(ref ParticleRendererSettings settings)
    {
        float progress = (float)TimeLeft / MaxTime;

        Velocity *= 0.9f;
        Position += Velocity;

        if (HostPlayer != null)
        {
            Vector2 difference = (HostPlayer.position - OldPos) / 3f;

            if (difference.Length() > 200)
                Position += difference;
            else
                Position += difference * (1f - progress * StrayPercent);

            OldPos = HostPlayer.position;
            float distance = Position.Distance(HostPlayer.MountedCenter / 3f);
            if (distance > 150)
                TimeLeft++;

            if (distance > 300)
                ShouldBeRemovedFromRenderer = true;
        }

        if (TimeLeft++ > MaxTime)
            ShouldBeRemovedFromRenderer = true;
    }

    public static Asset<Texture2D>[] FrenziedParticleTexture;

    public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
    {
        float progress = (float)TimeLeft / MaxTime;

        float fadeIn = MathF.Sin(Utils.GetLerpValue(0, 60, TimeLeft, true) * MathHelper.PiOver2);

        int frameCounter = (int)Math.Floor(StartFrame * fadeIn + progress * 140);
        Texture2D texture = FrenziedParticleTexture[FlameStyle].Value;
        Rectangle frame = texture.Frame(16, 12, frameCounter % 16, frameCounter / 16);

        SpriteEffects effect = Pinkness > 50 ? SpriteEffects.FlipHorizontally : 0;

        float scaleMod = (0.6f + MathF.Pow(progress, 2) * 0.5f) * fadeIn;
        Color drawColor = new Color(Pinkness / 100f, 1f, progress * (fadeIn * 0.5f + 0.5f));

        if (HostPlayer != null)
            scaleMod += Utils.GetLerpValue(20, 700, Position.Distance(HostPlayer.MountedCenter / 3f), true) * fadeIn;

        Vector2 stretch = new Vector2(1f, 0.7f + Velocity.Length() * 0.1f);
        spritebatch.Draw(texture, Position + settings.AnchorPosition, frame, drawColor, Rotation + MathHelper.PiOver2, frame.Size() * new Vector2(0.5f, 0.66f), Scale * scaleMod * stretch, effect, 0);
    }

    public void Load(Mod mod)
    {
        FrenziedParticleTexture = AllAssets.RequestArray<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Particles/FrenziedParticle_", 3);
    }

    public void Unload()
    {
    }
}