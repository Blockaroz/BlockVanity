using BlockVanity.Common.Graphics;
using BlockVanity.Common.UI;
using BlockVanity.Content.Items.Vanity.Midra;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
        Main.QueueMainThreadAction(() =>
        {
            drawToTarget += DrawParticlesToTarget;
            frenziedParticles = new MonoParticleSystem<FrenziedFlameParticle>(200);
            frenziedTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, backSize, backSize);
            frenziedTargetFront = new RenderTarget2D(Main.graphics.GraphicsDevice, frontSize, frontSize);
        });
    }

    public void DrawParticlesToTarget(SpriteBatch spriteBatch)
    {
        if (Player != null)
        {
            if (!IsReady())
                return;

            frenziedParticles.RenderSettings.AnchorPosition = new Vector2(backSize / 2) - GetOffsetAnchor(Player);

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            Effect particleEffect = AllAssets.Effects.FrenziedFlameParticle.Value;
            particleEffect.Parameters["uPower"].SetValue(2.2f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(200, 25, 1, 190).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(215, 200, 40, 255).ToVector4());
            particleEffect.Parameters["uAltColor"].SetValue(new Color(115, 0, 255, 170).ToVector4());

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            frenziedParticles.Draw(spriteBatch, VanityUtils.MaxBlendState, Main.GameViewMatrix.EffectMatrix, particleEffect);

            particleEffect.Parameters["uPower"].SetValue(2.5f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(180, 15, 1, 30).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(150, 150, 60, 155).ToVector4());
            particleEffect.Parameters["uAltColor"].SetValue(new Color(45, 0, 255, 120).ToVector4());

            frenziedParticles.Draw(spriteBatch, BlendState.Additive, Main.GameViewMatrix.EffectMatrix, particleEffect);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            Texture2D giantBloom = AllAssets.Textures.Glow[0].Value;
            spriteBatch.Draw(giantBloom, new Vector2(backSize / 2), giantBloom.Frame(), Color.Orange with { A = 10 } * 0.33f, 0f, giantBloom.Size() * 0.5f, 1f, 0, 0);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedTargetFront);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            Matrix normalizedTransform = Matrix.Invert(Main.GameViewMatrix.EffectMatrix) * Matrix.CreateOrthographicOffCenter(0, frontSize, frontSize, 0, 0, 1);
            Effect eyeEffect = AllAssets.Effects.FrenziedFlameEye.Value;
            eyeEffect.Parameters["transformMatrix"].SetValue(normalizedTransform);
            eyeEffect.Parameters["uTexture"].SetValue(AllAssets.Textures.MiscNoise[2].Value);
            eyeEffect.Parameters["uColor"].SetValue(new Color(255, 180, 0, 100).ToVector4());
            eyeEffect.Parameters["uSecondaryColor"].SetValue(new Color(255, 190, 0, 10).ToVector4());
            eyeEffect.Parameters["uTime"].SetValue(miscTimer / 400f);
            eyeEffect.Parameters["uVertexDistortion"].SetValue(0.5f);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eyeEffect, Main.GameViewMatrix.EffectMatrix);

            Texture2D blackTile = TextureAssets.BlackTile.Value;
            spriteBatch.Draw(blackTile, new Vector2(frontSize / 2), blackTile.Frame(), Color.Black, 0f, blackTile.Size() * 0.5f, 1.75f, 0, 0);

            spriteBatch.End();
        }
    }

    public bool IsReady() => frenziedTarget != null && frenziedParticles != null;
    public DrawData GetFrenzyTarget() => new DrawData(frenziedTarget, Vector2.Zero, frenziedTarget.Frame(), Color.White, -Player.fullRotation, frenziedTarget.Size() * 0.5f, 1f, 0);
    public DrawData GetFrenzyTargetFront() => new DrawData(frenziedTargetFront, Vector2.Zero, frenziedTargetFront.Frame(), Color.White, -Player.fullRotation, frenziedTargetFront.Size() * 0.5f, 1f, 0);

    public static Vector2 GetOffsetAnchor(Player player) => player?.MountedCenter / 3f ?? Vector2.Zero;

    public bool forceFlameBack;

    public float miscTimer;

    public override void FrameEffects()
    {
        if (Main.gameInactive || !IsReady())
            return;

        UpdateCape();

        if (Player.head == EquipLoader.GetEquipSlot(Mod, nameof(AshenHead), EquipType.Head) || forceFlameBack)
        {
            if (AreaEffectsToggle.IsActive(Player))
            {
                Lighting.AddLight(Player.MountedCenter, Color.OrangeRed.ToVector3() * 0.5f);

                if (Player.timeSinceLastDashStarted < 5 && Player.timeSinceLastDashStarted > 0 && Player.velocity.Length() > 0.1f)
                {
                    FrenziedFlameParticle particle = frenziedParticles.RequestParticle();
                    particle.Prepare(GetOffsetAnchor(Player) - Player.velocity, Player.velocity.RotatedByRandom(0.2f) * 0.15f, 0.66f + Main.rand.NextFloat(0.3f), Main.rand.Next(100, 120), 0.3f, Player);
                    frenziedParticles.Particles.Add(particle);
                }
            }

            if (miscTimer % 2 == 0)
            {
                Vector2 particleVelocity = Main.rand.NextVector2Circular(1, 1) - Vector2.UnitY * 0.15f;
                Vector2 particlePosition = GetOffsetAnchor(Player) + Player.velocity * 0.4f;
                FrenziedFlameParticle particle = frenziedParticles.RequestParticle();
                particle.Prepare(particlePosition, particleVelocity * 0.15f, 0.3f + Main.rand.NextFloat(0.4f), Main.rand.Next(140, 180), 0.1f + Main.rand.NextFloat(0.2f), Player);
                frenziedParticles.Particles.Add(particle);
            }
        }

        frenziedParticles.Update();

        if (++miscTimer > 1200)
            miscTimer = 0;
    }

    public void UpdateCape()
    {

    }

    public override void ResetEffects()
    {
        forceFlameBack = false;
    }
}