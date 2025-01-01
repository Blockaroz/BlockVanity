using System;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Particles;
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

    public ParticleSystem frenziedParticles;

    private RenderTarget2D frenziedTarget;

    private static event Action<SpriteBatch> drawToTarget;

    public override void Initialize()
    {
        frenziedParticles = new ParticleSystem(400);

        Main.QueueMainThreadAction(() =>
        {
            frenziedTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 800, 800);
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
        spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTarget);
        spriteBatch.GraphicsDevice.Clear(Color.Transparent);

        if (IsReady && Player != null)
        {
            //if (Player.head != EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head))
            //    return;

            const float rescale = 1f;
            Vector2 center = GetOffsetAnchor(Player) - new Vector2(400 * rescale);
            Matrix transform = Matrix.CreateScale(1f / rescale) * Main.GameViewMatrix.EffectMatrix;

            Effect particleEffect = AllAssets.Effects.DistortDissolve.Value;
            particleEffect.Parameters["uTexture"].SetValue(AllAssets.Textures.FireDissolveNoise[0].Value);
            particleEffect.Parameters["uNoiseSpeed"].SetValue(0.4f);
            particleEffect.Parameters["uNoiseStrength"].SetValue(0.67f);
            particleEffect.Parameters["uNoiseScale"].SetValue(new Vector2(0.2f));
            particleEffect.Parameters["uPower"].SetValue(55f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(170, 0, 21, 200).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(255, 200, 10, 255).ToVector4());
            frenziedParticles.Draw(spriteBatch, center, AddAlphaBlend, transform, particleEffect);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, transform);

            Texture2D eyeTexture = AllAssets.Textures.FrenziedFlameLordEye.Value;
            spriteBatch.Draw(eyeTexture, new Vector2(400 * rescale), eyeTexture.Frame(), Color.White, Player.miscCounterNormalized * MathHelper.TwoPi, eyeTexture.Size() * 0.5f, 0.11f, 0, 0);

            Texture2D glowTexture = AllAssets.Textures.Glow[0].Value;
            spriteBatch.Draw(glowTexture, new Vector2(400 * rescale), glowTexture.Frame(), new Color(50, 30, 21, 0), Player.miscCounterNormalized * MathHelper.TwoPi, glowTexture.Size() * 0.5f, 1f, 0, 0);

            spriteBatch.End();
        }
    }

    public int targetShader;
    public bool IsReady => frenziedTarget != null && frenziedParticles != null;
    public DrawData GetFrenzyTarget() => new DrawData(frenziedTarget, Vector2.Zero, frenziedTarget.Frame(), Color.White, -Player.fullRotation, frenziedTarget.Size() * 0.5f, 1f, 0);

    private static Vector2 GetOffsetAnchor(Player player) => player?.MountedCenter / 3f ?? Vector2.Zero;

    public override void FrameEffects()
    {
        if (Main.gameInactive || frenziedParticles == null)
            return;

        frenziedParticles.Update();

        Vector2 particlePosition = GetOffsetAnchor(Player) + Player.velocity * 0.2f;
        Vector2 particleVelocity = Main.rand.NextVector2Circular(1, 1) * 0.3f - Vector2.UnitY * 0.1f + Player.velocity * 0.001f;
        frenziedParticles.NewParticle(new FrenziedFlameParticle(Main.rand.Next(200, 250), Player), particlePosition, particleVelocity, 0, 0.5f + Main.rand.NextFloat());

        targetShader = Player.cHead;
    }
}
