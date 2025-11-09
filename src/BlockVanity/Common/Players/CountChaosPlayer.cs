using BlockVanity.Common.Graphics;
using BlockVanity.Content.Items.Vanity.CountChaos;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class CountChaosPlayer : ModPlayer
{
    public override void Load()
    {
        On_Main.CheckMonoliths += DrawAllTargets;
        On_Player.SetArmorEffectVisuals += ArmorShadows;
    }

    private void DrawAllTargets(On_Main.orig_CheckMonoliths orig)
    {
        drawToTarget?.Invoke(Main.spriteBatch);

        Main.spriteBatch.GraphicsDevice.SetRenderTarget(null);
        Main.spriteBatch.GraphicsDevice.Clear(Color.Transparent);

        orig();
    }

    public MonoParticleSystem<ChaosMatterParticle> particles;

    private RenderTarget2D chaosParticlePreTarget;
    private RenderTarget2D chaosParticleTarget;

    private static event Action<SpriteBatch> drawToTarget;

    private const int targetSize = 240;

    public override void Initialize()
    {
        particles = new MonoParticleSystem<ChaosMatterParticle>(200);
    }

    public static readonly BlendState MinimumColorBlend = new BlendState()
    {
        AlphaBlendFunction = BlendFunction.Max,
        ColorBlendFunction = BlendFunction.Min,
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.One,
        AlphaSourceBlend = Blend.One,
        AlphaDestinationBlend = Blend.One
    };

    public void PrepareTarget()
    {
        Main.QueueMainThreadAction(() =>
        {
            chaosParticlePreTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize / 2, targetSize / 2);
            chaosParticleTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize, targetSize);
            drawToTarget += s => DrawParticlesToTarget(Player, s);
        });
    }

    public void DrawParticlesToTarget(Player player, SpriteBatch spriteBatch)
    {
        CountChaosPlayer chaosPlayer = player.GetModPlayer<CountChaosPlayer>();
        
        if (chaosPlayer.needsTarget && chaosPlayer.IsReady())
        {
            //bool head = Player.head == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head);
            //bool body = Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);
            //bool legs = Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);
            //if (!(body || legs))
            //    return;

            const float rescale = 0.5f;
            Vector2 anchor = GetOffsetAnchor(player) - new Vector2(targetSize / 4 / rescale);
            Matrix transform = Matrix.CreateScale(rescale) * Main.GameViewMatrix.EffectMatrix;

            Effect colorOnly = AllAssets.Effects.TransparencyMask.Value;
            colorOnly.Parameters["uColor"].SetValue(Vector3.One);

            spriteBatch.GraphicsDevice.SetRenderTarget(chaosPlayer.chaosParticlePreTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            chaosPlayer.particles.RenderSettings.AnchorPosition = -anchor;
            chaosPlayer.particles.Draw(spriteBatch, BlendState.AlphaBlend, transform, colorOnly);
            chaosPlayer.particles.Draw(spriteBatch, MinimumColorBlend, transform, null);

            spriteBatch.GraphicsDevice.SetRenderTarget(chaosPlayer.chaosParticleTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            spriteBatch.Draw(chaosPlayer.chaosParticlePreTarget, new Vector2(targetSize / 2), null, Color.White, 0f, chaosPlayer.chaosParticlePreTarget.Size() / 2, 1f, Main.GameViewMatrix.Effects, 0);
            
            spriteBatch.End();
        }
    }

    public int targetShader;
    public bool IsReady() => chaosParticlePreTarget != null && chaosParticleTarget != null && particles != null;
    public DrawData GetChaosParticleTarget() => new DrawData(chaosParticleTarget, Player.MountedCenter - Main.screenPosition, chaosParticleTarget.Frame(), Color.White, -Player.fullRotation, chaosParticleTarget.Size() * 0.5f, 2f, 0);

    public static Vector2 GetOffsetAnchor(Player player) => player.Center / 16f;

    public bool needsTarget;

    public bool RequestTargetIfNeeded()
    {
        needsTarget = true;

        if (!IsReady())
        {
            PrepareTarget();
            return false;
        }

        return true;
    }

    public override void FrameEffects()
    {
        if (Main.gameInactive)
            return;

        if (Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs))
        {
            if (!RequestTargetIfNeeded())
                return;

            Vector2 legVel = new Vector2(-0.3f * Player.direction, Main.rand.NextFloat(0.8f, 1.2f) * Player.gravDir).RotatedByRandom(0.2f);
            Vector2 legPos = GetOffsetAnchor(Player) + new Vector2(0, 14 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.3f;
            Vector2 legGrav = new Vector2(-Player.direction * 0.01f, -0.1f * Player.gravDir);

            ChaosMatterParticle particle = particles.RequestParticle();
            particle.Prepare(legPos, Player.velocity * 0.05f + legVel, legGrav, Main.rand.Next(25, 40), Main.rand.Next(-2, 3) * MathHelper.PiOver2, 0.5f + Main.rand.NextFloat(0.5f));
            particles.Particles.Add(particle);
            targetShader = Player.cLegs;
        }

        if (Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body))
        {
            if (!RequestTargetIfNeeded())
                return;

            Vector2 bodyVel = new Vector2(Main.rand.NextFloat(-0.7f, 0.2f) * Player.direction, -Main.rand.NextFloat(-0.2f, 0.5f) * Player.gravDir);
            Vector2 bodyPos = GetOffsetAnchor(Player) + Main.rand.NextVector2Circular(6, 10) + new Vector2(-6 * Player.direction, -6 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.5f;
            Vector2 bodyGrav = -Vector2.UnitY * Main.rand.NextFloat(0.05f, 0.08f) * Player.gravDir;

            ChaosMatterParticle particle = particles.RequestParticle();
            particle.Prepare(bodyPos, -Player.velocity * Main.rand.NextFloat(0.1f) + bodyVel, bodyGrav, Main.rand.Next(25, 40), Main.rand.Next(-2, 3) * MathHelper.PiOver2, 0.9f + Main.rand.NextFloat(0.3f));
            particles.Particles.Add(particle);
            targetShader = Player.cBody;
        }

        particles.Update();
    }

    public override void ResetEffects()
    {
        needsTarget = false;
    }

    private void ArmorShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        bool head = self.head == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head);
        bool body = self.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);
        bool legs = self.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);
        if (head && body && legs)
        {
            self.armorEffectDrawShadowSubtle = true;
        }
        else
        {
            orig(self, drawPlayer);
        }
    }
}