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

    public MonoParticleSystem<ChaosMatterParticle> chaosParticles;

    private RenderTarget2D chaosFireTarget;

    private static event Action<SpriteBatch> drawToTarget;

    public override void Initialize()
    {
        chaosParticles = new MonoParticleSystem<ChaosMatterParticle>(200);

        Main.QueueMainThreadAction(() =>
        {
            chaosFireTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 400, 400);
            drawToTarget += DrawParticlesToTarget;
        });
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

    public void DrawParticlesToTarget(SpriteBatch spriteBatch)
    {
        spriteBatch.GraphicsDevice.SetRenderTarget(chaosFireTarget);
        spriteBatch.GraphicsDevice.Clear(Color.Transparent);

        if (IsReady && Player != null)
        {
            //bool head = Player.head == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head);
            //bool body = Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);
            //bool legs = Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);
            //if (!(body || legs))
            //    return;

            const float rescale = 0.5f;
            Vector2 anchor = GetOffsetAnchor(Player) - new Vector2(200 / rescale);
            Matrix transform = Matrix.CreateScale(rescale) * Main.GameViewMatrix.EffectMatrix;

            Effect colorOnly = AllAssets.Effects.TransparencyMask.Value;
            colorOnly.Parameters["uColor"].SetValue(Vector3.One);

            chaosParticles.RenderSettings.AnchorPosition = -anchor;
            chaosParticles.Draw(spriteBatch, BlendState.AlphaBlend, transform, colorOnly);
            chaosParticles.Draw(spriteBatch, MinimumColorBlend, transform, null);
        }
    }

    public int targetShader;
    public bool IsReady => chaosFireTarget != null && chaosParticles != null;
    public DrawData GetChaosFireTarget() => new DrawData(chaosFireTarget, Player.MountedCenter - Main.screenPosition, chaosFireTarget.Frame(), Color.White, -Player.fullRotation, chaosFireTarget.Size() * 0.5f, 2f, 0);

    private static Vector2 GetOffsetAnchor(Player player) => player?.MountedCenter / 16f ?? Vector2.Zero;

    public override void FrameEffects()
    {
        if (Main.gameInactive)
        {
            return;
        }

        chaosParticles.Update();

        if (Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs))
        {
            Vector2 legVel = new Vector2(-0.3f * Player.direction, Main.rand.NextFloat(0.8f, 1.2f) * Player.gravDir).RotatedByRandom(0.2f);
            Vector2 legPos = GetOffsetAnchor(Player) + new Vector2(0, 14 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.3f;
            Vector2 legGrav = new Vector2(-Player.direction * 0.01f, -0.1f * Player.gravDir);

            ChaosMatterParticle particle = chaosParticles.RequestParticle();
            particle.Prepare(legPos, Player.velocity * 0.05f + legVel, legGrav, Main.rand.Next(25, 40), Main.rand.Next(-2, 3) * MathHelper.PiOver2, 0.5f + Main.rand.NextFloat(0.5f));
            chaosParticles.Particles.Add(particle);
            targetShader = Player.cLegs;
        }

        if (Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body))
        {
            Vector2 bodyVel = new Vector2(Main.rand.NextFloat(-0.7f, 0.2f) * Player.direction, -Main.rand.NextFloat(-0.2f, 0.5f) * Player.gravDir);
            Vector2 bodyPos = GetOffsetAnchor(Player) + Main.rand.NextVector2Circular(6, 10) + new Vector2(-6 * Player.direction, -6 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.5f;
            Vector2 bodyGrav = -Vector2.UnitY * Main.rand.NextFloat(0.05f, 0.08f) * Player.gravDir;

            ChaosMatterParticle particle = chaosParticles.RequestParticle();
            particle.Prepare(bodyPos, -Player.velocity * Main.rand.NextFloat(0.1f) + bodyVel, bodyGrav, Main.rand.Next(25, 40), Main.rand.Next(-2, 3) * MathHelper.PiOver2, 0.9f + Main.rand.NextFloat(0.3f));
            chaosParticles.Particles.Add(particle);
            targetShader = Player.cBody;
        }
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