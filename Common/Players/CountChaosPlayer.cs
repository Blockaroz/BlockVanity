using System;
using System.Collections.Generic;
using BlockVanity.Common.Graphics.ParticleRendering;
using BlockVanity.Content.Items.Vanity.CountChaos;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class CountChaosPlayer : ModPlayer
{
    public override void Load()
    {
        On_Main.CheckMonoliths += DrawAllTargets;
        On_Player.PlayerFrame += SlowLegs;
        On_Player.SetArmorEffectVisuals += ArmorShadows;
    }

    private void DrawAllTargets(On_Main.orig_CheckMonoliths orig)
    {
        drawToTarget?.Invoke(Main.spriteBatch);

        Main.spriteBatch.GraphicsDevice.SetRenderTarget(null);
        Main.spriteBatch.GraphicsDevice.Clear(Color.Transparent);

        orig();
    }

    public ParticleSystem chaosFireParticles;

    private RenderTarget2D chaosFireTarget;

    private static event Action<SpriteBatch> drawToTarget;

    public override void Initialize()
    {
        chaosFireParticles = new ParticleSystem(200, false);
        chaosFireParticles.Init();

        Main.QueueMainThreadAction(() =>
        {
            chaosFireTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 300, 300);
        });

        drawToTarget += DrawParticlesToTarget;
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

        if (chaosFireParticles != null)
        {
            const float rescale = 0.5f;
            Vector2 anchor = OffsetAnchor - new Vector2(150 / rescale);
            Matrix transform = Matrix.CreateScale(rescale) * Main.GameViewMatrix.EffectMatrix;

            if (chaosFireParticles.Particles.Count <= 0)
                return;

            List<Particle> normalParticles = new List<Particle>();

            foreach (Particle particle in chaosFireParticles.Particles)
            {
                if (particle.active)
                    normalParticles.Add(particle);
            }

            Effect colorOnly = AllAssets.Effects.TransparencyMask.Value;
            colorOnly.Parameters["uColor"].SetValue(Vector3.One);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, colorOnly, transform);

            if (normalParticles.Count > 0)
            {
                foreach (Particle particle in normalParticles)
                    particle.Draw(spriteBatch, anchor);
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, MinimumColorBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, transform);

            if (normalParticles.Count > 0)
            {
                foreach (Particle particle in normalParticles)
                    particle.Draw(spriteBatch, anchor);
            }

            spriteBatch.End();
        }
    }

    public int flameShader;
    public bool IsReady => chaosFireTarget != null;
    public DrawData GetChaosFire() => new DrawData(chaosFireTarget, Player.MountedCenter - Main.screenPosition, chaosFireTarget.Frame(), Color.White, -Player.fullRotation, chaosFireTarget.Size() * 0.5f, 2f, 0);

    private Vector2 OffsetAnchor => Player.MountedCenter / 32f;

    public override void FrameEffects()
    {
        if (Main.gameInactive)
            return;

        chaosFireParticles.Update();

        if (Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs))
        {
            Vector2 legVel = new Vector2(-0.2f * Player.direction, Main.rand.NextFloat(0.8f, 1.2f) * Player.gravDir).RotatedByRandom(0.2f);
            Vector2 legPos = OffsetAnchor + new Vector2(2 * Player.direction, 12 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.1f;
            Vector2 legGrav = new Vector2(-Player.direction * 0.04f, -0.07f * Player.gravDir);
            chaosFireParticles.NewParticle(new ChaosFlameParticle(Main.rand.Next(20, 30), legGrav), legPos, Player.velocity * 0.05f + legVel, Main.rand.Next(4) * MathHelper.PiOver2, Main.rand.NextFloat(0.5f, 0.7f));
            flameShader = Player.cLegs;
        }

        if (Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body))
        {
            Vector2 particleVel = new Vector2(Main.rand.NextFloat(-0.4f, 0.3f) * Player.direction, -Main.rand.NextFloat(-0.2f, 0.5f) * Player.gravDir);
            Vector2 particlePos = OffsetAnchor + Main.rand.NextVector2Circular(6, 10) + new Vector2(-8 * Player.direction, -6 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.3f;
            chaosFireParticles.NewParticle(new ChaosFlameParticle(Main.rand.Next(30, 40), -Vector2.UnitY * Main.rand.NextFloat(0.05f, 0.08f) * Player.gravDir), particlePos, -Player.velocity * Main.rand.NextFloat(0.1f) + particleVel, Main.rand.Next(4) * MathHelper.PiOver2, Main.rand.NextFloat(1f, 1.2f));
            flameShader = Player.cBody;
        }
    }

    internal float walkCounter;
    internal int walkFrame;

    private void SlowLegs(On_Player.orig_PlayerFrame orig, Player self)
    {
        orig(self);

        if (self.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs) && self.velocity.Y == 0)
        {
            CountChaosPlayer chaosPlayer = self.GetModPlayer<CountChaosPlayer>();

            if (!Main.gameInactive)
                chaosPlayer.walkCounter += Math.Abs(self.velocity.X * 0.275f);

            while (chaosPlayer.walkCounter > 8)
            {
                chaosPlayer.walkCounter -= 8;
                chaosPlayer.walkFrame += self.legFrame.Height;
            }

            if (chaosPlayer.walkFrame < self.legFrame.Height * 7)
                chaosPlayer.walkFrame = self.legFrame.Height * 19;
            else if (chaosPlayer.walkFrame > self.legFrame.Height * 19)
                chaosPlayer.walkFrame = self.legFrame.Height * 7;

            if (self.velocity.X == 0)
            {
                chaosPlayer.walkFrame = 0;
                chaosPlayer.walkCounter = 0;
            }

            self.bodyFrameCounter = 0.0;
            self.legFrameCounter = 0.0;
            self.legFrame.Y = chaosPlayer.walkFrame;
        }
    }

    private void ArmorShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        bool head = self.head == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head);
        bool body = self.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);
        bool legs = self.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);
        if (head && body && legs)
            self.armorEffectDrawShadowSubtle = true;

        else
            orig(self, drawPlayer);
    }

}
