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

    public MonoParticleSystem<FrenziedFlameParticle> particles;

    private RenderTarget2D frenzyHeadPreTarget;
    private RenderTarget2D frenzyHeadTarget;

    private static event Action<SpriteBatch> drawToTarget;

    private const int targetSize = 360;

    public override void Initialize()
    {
        particles = new MonoParticleSystem<FrenziedFlameParticle>(200);
    }

    public void PrepareTarget()
    {
        Main.QueueMainThreadAction(() =>
        {
            frenzyHeadPreTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize / 2, targetSize / 2);
            frenzyHeadTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize, targetSize);
            drawToTarget += s => DrawParticlesToTarget(Player, s);
        });
    }

    public void DrawParticlesToTarget(Player player, SpriteBatch spriteBatch)
    {
        PlayerOfFrenziedFlame frenziedPlayer = player.GetModPlayer<PlayerOfFrenziedFlame>();

        if (frenziedPlayer.needsTarget && frenziedPlayer.IsReady())
        {
            const float rescale = 0.5f;
            Vector2 anchor = GetOffsetAnchor(Player) - new Vector2(targetSize / 4 / rescale);
            Matrix transform = Matrix.CreateScale(rescale) * Main.GameViewMatrix.EffectMatrix;

            frenziedPlayer.particles.RenderSettings.AnchorPosition = -anchor;

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedPlayer.frenzyHeadPreTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            Effect particleEffect = AllAssets.Effects.FrenziedFlameParticle.Value;
            particleEffect.Parameters["uPower"].SetValue(2.2f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(200, 25, 1, 190).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(215, 200, 40, 255).ToVector4());
            particleEffect.Parameters["uAltColor"].SetValue(new Color(115, 0, 255, 170).ToVector4());

            frenziedPlayer.particles.Draw(spriteBatch, VanityUtils.MaxBlendState, transform, particleEffect);

            particleEffect.Parameters["uPower"].SetValue(2.5f);
            particleEffect.Parameters["uDarkColor"].SetValue(new Color(150, 15, 1, 30).ToVector4());
            particleEffect.Parameters["uGlowColor"].SetValue(new Color(120, 120, 60, 155).ToVector4());
            particleEffect.Parameters["uAltColor"].SetValue(new Color(45, 0, 255, 120).ToVector4());

            frenziedPlayer.particles.Draw(spriteBatch, BlendState.Additive, transform, particleEffect);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            Texture2D giantBloom = AllAssets.Textures.Glow[0].Value;
            spriteBatch.Draw(giantBloom, new Vector2(targetSize / 4), giantBloom.Frame(), Color.Orange with { A = 10 } * 0.33f, 0f, giantBloom.Size() * 0.5f, 0.8f, 0, 0);

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(frenziedPlayer.frenzyHeadTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            spriteBatch.Draw(frenziedPlayer.frenzyHeadPreTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 2f, Main.GameViewMatrix.Effects, 0);

            spriteBatch.End();
        }
    }

    public bool IsReady() => frenzyHeadPreTarget != null && frenzyHeadTarget != null && particles != null;
    public DrawData GetFrenzyTarget() => new DrawData(frenzyHeadTarget, Vector2.Zero, frenzyHeadTarget.Frame(), Color.White, -Player.fullRotation, frenzyHeadTarget.Size() * 0.5f, 1f, 0);

    public static Vector2 GetOffsetAnchor(Player player) => player.Center / 3f;

    public bool needsTarget;

    public bool forceFlameBack;

    public float miscTimer;

    public override void FrameEffects()
    {
        if (Main.gameInactive)
            return;

        if (Player.head == EquipLoader.GetEquipSlot(Mod, nameof(AshenHead), EquipType.Head) || forceFlameBack)
        {
            needsTarget = true;

            if (!IsReady())
            {
                PrepareTarget();
                return;
            }

            if (AreaEffectsToggle.IsActive(Player))
                Lighting.AddLight(Player.MountedCenter, Color.Orange.ToVector3() * 0.5f);

            if (miscTimer % 2 == 0)
            {
                Vector2 particleVelocity = Main.rand.NextVector2Circular(1, 1) - Vector2.UnitY * 0.15f - Vector2.UnitX / 2 * Player.direction;
                Vector2 particlePosition = GetOffsetAnchor(Player) + Player.velocity * 0.4f;
                FrenziedFlameParticle particle = particles.RequestParticle();
                particle.Prepare(particlePosition, particleVelocity * 0.15f, 0.4f + Main.rand.NextFloat(0.3f), Main.rand.Next(80, 140), 0.2f + Main.rand.NextFloat(0.1f), Player);
                particles.Particles.Add(particle);
            }

            particles.Update();
        }

        if (++miscTimer > 1200)
            miscTimer = 0;
    }

    public override void ResetEffects()
    {
        needsTarget = false;
        forceFlameBack = false;
    }
}