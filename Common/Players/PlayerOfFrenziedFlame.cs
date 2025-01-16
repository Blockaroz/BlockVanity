using System;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Items.Vanity.Midra;
using BlockVanity.Content.Particles.SpecialParticles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class PlayerOfFrenziedFlame : ModPlayer
{
    public override void Load()
    {
        On_Main.CheckMonoliths += DrawAllTargets;
    }

    private void DrawAllTargets(On_Main.orig_CheckMonoliths orig)
    {
        drawToTarget?.Invoke(Main.spriteBatch);

        Main.spriteBatch.GraphicsDevice.SetRenderTarget(null);
        Main.spriteBatch.GraphicsDevice.Clear(Color.Transparent);

        orig();
    }

    public MonoParticleSystem<FrenziedFlameParticle> frenziedParticles;

    private RenderTarget2D frenziedTarget;
    private RenderTarget2D frenziedTargetFront;

    private static event Action<SpriteBatch> drawToTarget;

    private const int frontSize = 200;
    private const int backSize = 600;

    public override void Initialize()
    {
        frenziedParticles = new MonoParticleSystem<FrenziedFlameParticle>(200);

        Main.QueueMainThreadAction(() =>
        {
            frenziedTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, backSize, backSize);
            frenziedTargetFront = new RenderTarget2D(Main.graphics.GraphicsDevice, frontSize, frontSize);
            drawToTarget += DrawParticlesToTarget;
        });
    }

    public static readonly BlendState AddAlphaBlend = new BlendState()
    {
        AlphaBlendFunction = BlendFunction.Add,
        ColorBlendFunction = BlendFunction.Add,
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.One,
        AlphaSourceBlend = Blend.InverseDestinationAlpha,
        AlphaDestinationBlend = Blend.One
    };

    public void DrawParticlesToTarget(SpriteBatch spriteBatch)
    {
        if (IsReady && Player != null)
        {
            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            Effect particleEffect = AllAssets.Effects.FrenziedFlameParticle.Value;
            particleEffect.Parameters["uTexture"].SetValue(AllAssets.Textures.MiscNoise[0].Value);
            particleEffect.Parameters["uFrameCount"].SetValue(5);
            particleEffect.Parameters["uNoiseSpeed"].SetValue(0.3f);
            particleEffect.Parameters["uNoiseStrength"].SetValue(0.7f);
            particleEffect.Parameters["uNoiseScale"].SetValue(new Vector2(1f, 5f) * 0.5f);
            particleEffect.Parameters["uPower"].SetValue(1.5f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(45, 5, 0, 70).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(200, 180, 40, 255).ToVector4());
            particleEffect.Parameters["uAltColor"].SetValue(new Color(100, 0, 30, 120).ToVector4());

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            frenziedParticles.RenderSettings.AnchorPosition = new Vector2(backSize / 2) - GetOffsetAnchor(Player);
            frenziedParticles.Draw(spriteBatch, AddAlphaBlend, Main.GameViewMatrix.EffectMatrix, particleEffect);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            Texture2D giantBloom = AllAssets.Textures.Glow[0].Value;
            spriteBatch.Draw(giantBloom, new Vector2(backSize / 2), giantBloom.Frame(), Color.Goldenrod with { A = 10 }, 0f, giantBloom.Size() * 0.5f, 0.5f, 0, 0);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTargetFront);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            Matrix normalizedTransform = Matrix.Invert(Main.GameViewMatrix.EffectMatrix) * Matrix.CreateOrthographicOffCenter(0, frontSize, frontSize, 0, 0, 1);
            Effect eyeEffect = AllAssets.Effects.FrenziedFlameEye.Value;
            eyeEffect.Parameters["transformMatrix"].SetValue(normalizedTransform);
            eyeEffect.Parameters["uTexture"].SetValue(AllAssets.Textures.MiscNoise[2].Value);
            eyeEffect.Parameters["uColor"].SetValue(new Color(255, 180, 0, 100).ToVector4());
            eyeEffect.Parameters["uSecondaryColor"].SetValue(new Color(255, 190, 0, 10).ToVector4());
            eyeEffect.Parameters["uTime"].SetValue(miscTimer / 240f);
            eyeEffect.Parameters["uVertexDistortion"].SetValue(1.5f);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eyeEffect, Main.GameViewMatrix.EffectMatrix);

            Texture2D blackTile = TextureAssets.BlackTile.Value;
            spriteBatch.Draw(blackTile, new Vector2(frontSize / 2), blackTile.Frame(), Color.Black, 0f, blackTile.Size() * 0.5f, 2f, 0, 0);

            spriteBatch.End();
        }
    }

    public bool IsReady => frenziedTarget != null && frenziedParticles != null;
    public DrawData GetFrenzyTarget() => new DrawData(frenziedTarget, Vector2.Zero, frenziedTarget.Frame(), Color.White, -Player.fullRotation, frenziedTarget.Size() * 0.5f, 1f, 0);
    public DrawData GetFrenzyTargetFront() => new DrawData(frenziedTargetFront, Vector2.Zero, frenziedTargetFront.Frame(), Color.White, -Player.fullRotation, frenziedTargetFront.Size() * 0.5f, 1f, 0);

    private static Vector2 GetOffsetAnchor(Player player) => player?.MountedCenter / 3f ?? Vector2.Zero;

    public bool forceFlameBack;

    public Vector2 capeTargetVector;
    public float miscTimer;

    public override void FrameEffects()
    {
        if (Main.gameInactive || !IsReady)
            return;

        if (++miscTimer > 1200)
            miscTimer = 0;

        if (Player.head == EquipLoader.GetEquipSlot(Mod, nameof(EyeOfFrenziedFlame), EquipType.Head) || forceFlameBack)
        {
            frenziedParticles.Update();

            Vector2 particlePosition = GetOffsetAnchor(Player) + Player.velocity * 0.2f;
            Vector2 particleVelocity = Main.rand.NextVector2Circular(1, 1) * 0.33f - Vector2.UnitY * 0.1f;
            FrenziedFlameParticle particle = frenziedParticles.RequestParticle();
            particle.Prepare(particlePosition, particleVelocity, 0.4f + Main.rand.NextFloat(0.6f), Main.rand.Next(100, 200), 0.1f + Main.rand.NextFloat(0.3f), Player);
            frenziedParticles.Particles.Add(particle);
        }
    }

    public override void ResetEffects()
    {

    }
}
