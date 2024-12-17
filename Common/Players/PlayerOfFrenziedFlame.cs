using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Items.Vanity.CountChaos;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;

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

    public ParticleSystem<FrenziedFlameParticle> frenziedParticles;

    private RenderTarget2D frenziedTarget;

    private static event Action<SpriteBatch> drawToTarget;

    public override void Initialize()
    {
        frenziedParticles = new ParticleSystem<FrenziedFlameParticle>(200);
        frenziedParticles.Init();

        Main.QueueMainThreadAction(() =>
        {
            frenziedTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 400, 400);
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
            //bool head = Player.head == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head);
            //bool body = Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);
            //bool legs = Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);
            //if (!(body || legs))
            //    return;

            const float rescale = 1f;
            Vector2 anchor = GetOffsetAnchor(Player) - new Vector2(200 * rescale);
            Matrix transform = Matrix.CreateScale(1f / rescale) * Main.GameViewMatrix.EffectMatrix;

            if (frenziedParticles.Particles.Count <= 0)
                return;

            spriteBatch.Begin(SpriteSortMode.Deferred, AddAlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, transform);

            frenziedParticles.Draw(spriteBatch, anchor);

            spriteBatch.End();
        }
    }

    public int targetShader;
    public bool IsReady => frenziedTarget != null && frenziedParticles != null;
    public DrawData GetFrenzyTarget() => new DrawData(frenziedTarget, Player.MountedCenter - Main.screenPosition, frenziedTarget.Frame(), Color.White, -Player.fullRotation, frenziedTarget.Size() * 0.5f, 1f, 0);

    private static Vector2 GetOffsetAnchor(Player player) => player?.MountedCenter / 24f ?? Vector2.Zero;

    public override void FrameEffects()
    {
        if (Main.gameInactive)
            return;

        frenziedParticles.Update();

        if (true)
        {
            Vector2 particleVelocity = Main.rand.NextVector2Circular(1, 1) * 0.7f - Vector2.UnitY * 0.1f + Player.velocity * 0.01f;
            frenziedParticles.NewParticle(new FrenziedFlameParticle(140), GetOffsetAnchor(Player) + Player.velocity * 0.3f - new Vector2(0, 17), particleVelocity, 0f, 0.5f + Main.rand.NextFloat());
            targetShader = Player.cHead;
        }
    }
}
