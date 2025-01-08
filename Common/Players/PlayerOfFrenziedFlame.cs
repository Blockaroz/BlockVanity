using System;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Items.Vanity.Midra;
using BlockVanity.Content.Particles.SpecialParticles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
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

    public NotFastParticleSystem frenziedParticles;
    public NotFastParticleSystem frenziedParticlesFront;

    private RenderTarget2D frenziedTarget;
    private RenderTarget2D frenziedTargetFront;

    private static event Action<SpriteBatch> drawToTarget;

    public override void Initialize()
    {
        frenziedParticles = new NotFastParticleSystem(300);
        frenziedParticlesFront = new NotFastParticleSystem(300);

        Main.QueueMainThreadAction(() =>
        {
            frenziedTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 1200, 1200);
            frenziedTargetFront = new RenderTarget2D(Main.graphics.GraphicsDevice, 400, 400);
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
            if (Player.head != EquipLoader.GetEquipSlot(Mod, nameof(EyeOfFrenziedFlame), EquipType.Head))
                return;

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            const float rescale = 1f;
            Matrix transform = Matrix.CreateScale(1f / rescale) * Main.GameViewMatrix.EffectMatrix;

            Effect particleEffect = AllAssets.Effects.DistortDissolve.Value;
            particleEffect.Parameters["uTexture"].SetValue(AllAssets.Textures.FireDissolveNoise[0].Value);
            particleEffect.Parameters["uFrameCount"].SetValue(5);
            particleEffect.Parameters["uNoiseSpeed"].SetValue(0.4f);
            particleEffect.Parameters["uNoiseStrength"].SetValue(0.7f);
            particleEffect.Parameters["uNoiseScale"].SetValue(new Vector2(1f, 5f) * 0.5f);
            particleEffect.Parameters["uPower"].SetValue(8f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(42, 15, 5, 70).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(255, 205, 30, 200).ToVector4());
            particleEffect.Parameters["uAltColor"].SetValue(new Color(90, 0, 50, 170).ToVector4());
            frenziedParticles.Draw(spriteBatch, GetOffsetAnchor(Player) - new Vector2(600 * rescale), AddAlphaBlend, transform, particleEffect);

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTargetFront);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            // Need to recalculate fsr
            transform = Matrix.CreateScale(1f / rescale) * Main.GameViewMatrix.EffectMatrix;

            particleEffect.Parameters["uDarkColor"].SetValue(new Color(255, 150, 20, 50).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(255, 255, 200, 255).ToVector4());
            particleEffect.Parameters["uAltColor"].SetValue(new Color(255, 100, 30, 255).ToVector4());
            frenziedParticlesFront.Draw(spriteBatch, GetOffsetAnchor(Player) - new Vector2(200 * rescale), AddAlphaBlend, transform, particleEffect);

            spriteBatch.Begin(SpriteSortMode.Deferred, VanityUtils.MultiplyBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, transform);

            Texture2D eyeTexture = AllAssets.Textures.FrenziedFlameLordEye.Value;
            spriteBatch.Draw(eyeTexture, new Vector2(200 * rescale), null, Color.White, miscTimer / 600f * MathHelper.TwoPi, eyeTexture.Size() * 0.5f, 0.33f, 0, 0);

            spriteBatch.End();
        }
    }

    public float miscTimer;
    public int targetShader;
    public bool IsReady => frenziedTarget != null && frenziedParticles != null && frenziedParticlesFront != null;
    public DrawData GetFrenzyTarget() => new DrawData(frenziedTarget, Vector2.Zero, frenziedTarget.Frame(), Color.White, -Player.fullRotation, frenziedTarget.Size() * 0.5f, 1f, 0);
    public DrawData GetFrenzyTargetFront() => new DrawData(frenziedTargetFront, Vector2.Zero, frenziedTargetFront.Frame(), Color.White, -Player.fullRotation, frenziedTargetFront.Size() * 0.5f, 1f, 0);

    private static Vector2 GetOffsetAnchor(Player player) => player?.MountedCenter / 3f ?? Vector2.Zero;

    public override void FrameEffects()
    {
        if (Main.gameInactive || !IsReady)
            return;

        if (Player.head != EquipLoader.GetEquipSlot(Mod, nameof(EyeOfFrenziedFlame), EquipType.Head))
            return;
        
        miscTimer++;
        if (miscTimer > 1200)
            miscTimer = 0;

        frenziedParticles.Update();
        frenziedParticlesFront.Update();

        if (Player.miscCounter % 3 == 0)
        {
            Vector2 particlePosition = GetOffsetAnchor(Player) + Player.velocity * 0.15f;
            Vector2 particleVelocity = Main.rand.NextVector2Circular(1, 1);
            frenziedParticles.NewParticle(new FrenziedFlameParticle(Main.rand.Next(150, 300), Main.rand.NextFloat(0.1f, 0.4f), Player), particlePosition, particleVelocity, 0, 0.7f + Main.rand.NextFloat());
            
            frenziedParticlesFront.NewParticle(new FrenziedFlameParticle(Main.rand.Next(80, 150), 0.01f, Player), particlePosition, particleVelocity * 0.2f, 0, 0.7f);
        }

        targetShader = Player.cHead;
    }
}
