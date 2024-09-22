using System;
using BlockVanity.Common.Graphics;
using BlockVanity.Common.Graphics.ParticleRendering;
using BlockVanity.Content.Items.Vanity.CountChaos;
using BlockVanity.Content.Particles;
using log4net.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace BlockVanity.Common.Players;

public class CountChaosPlayer : ModPlayer
{
    public override void Load()
    {
        On_Main.CheckMonoliths += DrawAllTargets;
        On_Player.PlayerFrame += SlowLegs;
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
        chaosFireParticles = new ParticleSystem(500, false);
        chaosFireParticles.Init();

        Main.QueueMainThreadAction(() =>
        {
            chaosFireTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 600, 600);
        });

        drawToTarget += DrawParticlesToTarget;
    }

    public void DrawParticlesToTarget(SpriteBatch spriteBatch)
    {
        spriteBatch.GraphicsDevice.SetRenderTarget(chaosFireTarget);
        spriteBatch.GraphicsDevice.Clear(Color.Transparent);
        BlendState minimumColorBlend = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Max,
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One
        };
        chaosFireParticles?.Draw(spriteBatch, Player.MountedCenter / 4f - new Vector2(600), minimumColorBlend, Matrix.CreateScale(0.5f));
    }

    public override void FrameEffects()
    {
        chaosFireParticles.Update();

        if (Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs))
        {
            Vector2 legVel = new Vector2(0.1f * Player.direction, Main.rand.NextFloat(0.8f, 1.2f) * Player.gravDir).RotatedByRandom(0.2f);
            Vector2 legPos = Player.MountedCenter / 4f + new Vector2(2 * Player.direction, 12 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.5f;
            Vector2 legGrav = new Vector2(-Player.direction * 0.03f, -0.07f * Player.gravDir);
            chaosFireParticles.NewParticle(new ChaosFlameParticle(Main.rand.Next(20, 40), legGrav), legPos, Player.velocity * 0.1f + legVel, MathHelper.PiOver2 * Main.rand.Next(4), Main.rand.NextFloat(0.5f, 0.8f));
            flameShader = Player.cLegs;
        }
        if (Player.head == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head))
        {
            Vector2 particleVel = new Vector2(Main.rand.NextFloat(-0.4f, 0.3f) * Player.direction, -Main.rand.NextFloat(-0.2f, 0.5f) * Player.gravDir);
            Vector2 particlePos = Player.MountedCenter / 4f + Main.rand.NextVector2Circular(6, 8) + new Vector2(-7 * Player.direction, -10 * Player.gravDir).RotatedBy(Player.fullRotation);
            chaosFireParticles.NewParticle(new ChaosFlameParticle(Main.rand.Next(20, 40), -Vector2.UnitY * Main.rand.NextFloat(0.05f, 0.08f)), particlePos, Player.velocity * 0.15f + particleVel, MathHelper.PiOver2 * Main.rand.Next(4), Main.rand.NextFloat(1f, 1.1f));
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

            chaosPlayer.walkCounter += Math.Abs(self.velocity.X * 0.2f);
            while (chaosPlayer.walkCounter > 8)
            {
                chaosPlayer.walkCounter -= 8;
                chaosPlayer.walkFrame += self.legFrame.Height;
            }

            if (chaosPlayer.walkFrame < self.legFrame.Height * 7)
                chaosPlayer.walkFrame = self.legFrame.Height * 19;
            else if (chaosPlayer.walkFrame > self.legFrame.Height * 19)
                chaosPlayer.walkFrame = self.legFrame.Height * 7;

            self.bodyFrameCounter = 0.0;
            self.legFrameCounter = 0.0;
            self.legFrame.Y = chaosPlayer.walkFrame;
        }
    }

    public int flameShader;

    public bool IsReady => chaosFireTarget != null;

    public DrawData GetChaosFire() => new DrawData(chaosFireTarget, Player.MountedCenter - Main.screenPosition, chaosFireTarget.Frame(), Color.White, -Player.fullRotation, chaosFireTarget.Size() * 0.5f, 2f, 0, 0);
}
