using System;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Items.Vanity.Midra;
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
    public ParticleSystem frenziedParticlesFront;

    private RenderTarget2D frenziedTarget;
    private RenderTarget2D frenziedTargetFront;

    private static event Action<SpriteBatch> drawToTarget;

    public override void Initialize()
    {
        frenziedParticles = new ParticleSystem(300);
        frenziedParticlesFront = new ParticleSystem(300);

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

            const float rescale = 0.5f;
            Matrix transform = Matrix.CreateScale(1f / rescale) * Main.GameViewMatrix.EffectMatrix;

            Effect particleEffect = AllAssets.Effects.DistortDissolve.Value;
            particleEffect.Parameters["uTexture"].SetValue(AllAssets.Textures.FireDissolveNoise[0].Value);
            particleEffect.Parameters["uNoiseSpeed"].SetValue(0.4f);
            particleEffect.Parameters["uNoiseStrength"].SetValue(0.6f);
            particleEffect.Parameters["uNoiseScale"].SetValue(new Vector2(0.4f, -2f));
            particleEffect.Parameters["uPower"].SetValue(50f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(70, 10, 12, 150).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(255, 255, 5, 255).ToVector4());
            frenziedParticles.Draw(spriteBatch, GetOffsetAnchor(Player) - new Vector2(600 * rescale), AddAlphaBlend, transform, particleEffect);

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTargetFront);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            // Need to recalculate fsr
            transform = Matrix.CreateScale(1f / rescale) * Main.GameViewMatrix.EffectMatrix;

            particleEffect.Parameters["uDarkColor"].SetValue(new Color(255, 100, 20, 40).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(255, 255, 80, 255).ToVector4());
            frenziedParticlesFront.Draw(spriteBatch, GetOffsetAnchor(Player) - new Vector2(200 * rescale), AddAlphaBlend, transform, particleEffect);

            spriteBatch.Begin(SpriteSortMode.Deferred, VanityUtils.MultiplyBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, transform);

            Texture2D eyeTexture = AllAssets.Textures.FrenziedFlameLordEye.Value;
            spriteBatch.Draw(eyeTexture, new Vector2(200 * rescale), null, Color.White, miscTimer / 1200f * MathHelper.TwoPi, eyeTexture.Size() * 0.5f, 0.2f, 0, 0);

            spriteBatch.End();
        }
    }

    public float miscTimer;
    public int targetShader;
    public bool IsReady => frenziedTarget != null && frenziedParticles != null && frenziedParticlesFront != null;
    public DrawData GetFrenzyTarget() => new DrawData(frenziedTarget, Vector2.Zero, frenziedTarget.Frame(), Color.White, -Player.fullRotation, frenziedTarget.Size() * 0.5f, 0.5f, 0);
    public DrawData GetFrenzyTargetFront() => new DrawData(frenziedTargetFront, Vector2.Zero, frenziedTargetFront.Frame(), Color.White, -Player.fullRotation, frenziedTargetFront.Size() * 0.5f, 0.5f, 0);

    private static Vector2 GetOffsetAnchor(Player player) => player?.MountedCenter / 4f ?? Vector2.Zero;

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

        if (Player.miscCounter % 5 == 0)
        {
            Vector2 particlePosition = GetOffsetAnchor(Player) + Player.velocity * 0.2f;
            Vector2 particleVelocity = Main.rand.NextVector2Circular(1, 1) * 0.3f - Vector2.UnitY * 0.03f - Player.velocity * 0.01f;
            frenziedParticles.NewParticle(new FrenziedFlameParticle(Main.rand.Next(140, 300), Main.rand.NextFloat(0.3f, 0.5f), Player), particlePosition, particleVelocity, 0, 1f + Main.rand.NextFloat(2f));
            
            frenziedParticlesFront.NewParticle(new FrenziedFlameParticle(Main.rand.Next(90, 140), 0.1f, Player), particlePosition, particleVelocity * 0.1f, 0, 1f);
        }

        targetShader = Player.cHead;
    }
}
