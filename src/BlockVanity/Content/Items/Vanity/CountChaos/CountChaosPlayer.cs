using BlockVanity.Common.Graphics;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.CountChaos;

public class CountChaosPlayer : ModPlayer
{
    public static readonly BlendState MinimumColorBlend = new BlendState()
    {
        AlphaBlendFunction = BlendFunction.Max,
        ColorBlendFunction = BlendFunction.Min,
        ColorSourceBlend = Blend.One,
        ColorDestinationBlend = Blend.One,
        AlphaSourceBlend = Blend.One,
        AlphaDestinationBlend = Blend.One
    };

    public ParticleRenderer particles;

    private static RenderTarget2D ChaosPreTarget;
    private static RenderTarget2D ChaosTarget;

    private const int targetSize = 240;

    public override void Load()
    {
        Main.QueueMainThreadAction(() =>
        {
            ChaosPreTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize / 2, targetSize / 2);
            ChaosTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize, targetSize);
        });

        On_Player.SetArmorEffectVisuals += ArmorShadows;
    }

    public override void Initialize()
    {
        particles = new ParticleRenderer();
    }

    private void ArmorShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        bool head = drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosHornedHead), EquipType.Head);
        bool body = drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);
        bool legs = drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);

        if (head && body && legs)
            drawPlayer.armorEffectDrawShadowSubtle = true;
        else
            orig(self, drawPlayer);
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        bool active = Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs)
            || Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);

        if (!active)
            return;

        const float rescale = 0.5f;
        Vector2 anchor = GetOffsetAnchor(Player) - new Vector2(targetSize / 4 / rescale);
        Matrix transform = Matrix.CreateScale(rescale) * Main.GameViewMatrix.EffectMatrix;

        Effect colorOnly = Assets.Effects.TransparencyMask.Value;
        colorOnly.Parameters["uColor"].SetValue(Vector3.One);

        Rectangle prevScissor = Main.instance.GraphicsDevice.ScissorRectangle;
        RasterizerState prevRasterizer = Main.instance.GraphicsDevice.RasterizerState;
        Main.spriteBatch.End(out var snapshot);

        using (new RenderTargetScope(ChaosPreTarget, clear: true))
        {
            particles.Settings.AnchorPosition = -anchor;
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, colorOnly, transform);
            particles.Draw(Main.spriteBatch);
            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, MinimumColorBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, transform);
            particles.Draw(Main.spriteBatch);
            Main.spriteBatch.End();
        }

        using (new RenderTargetScope(ChaosTarget, clear: true))
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
            Main.spriteBatch.Draw(ChaosPreTarget, new Vector2(targetSize / 2), null, Color.White, 0f, ChaosPreTarget.Size() / 2, 1f, Main.GameViewMatrix.Effects, 0);
            Main.spriteBatch.End();
        }

        Main.spriteBatch.Begin(snapshot);
        Main.instance.GraphicsDevice.ScissorRectangle = prevScissor;
        Main.instance.GraphicsDevice.RasterizerState = prevRasterizer;
    }

    public int targetShader;

    public bool IsReady() => ChaosPreTarget != null && ChaosTarget != null && particles != null;

    public DrawData GetChaosParticleTarget() => new DrawData(ChaosTarget, Player.MountedCenter - Main.screenPosition, ChaosTarget.Frame(), Color.White, -Player.fullRotation, ChaosTarget.Size() * 0.5f, 2f, 0);

    public static Vector2 GetOffsetAnchor(Player player) => player.Center / 16f;

    public override void FrameEffects()
    {
        if (Main.gameInactive)
            return;

        if (Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs))
        {
            Vector2 legVel = new Vector2(-0.3f * Player.direction, Main.rand.NextFloat(0.8f, 1.2f) * Player.gravDir).RotatedByRandom(0.2f);
            Vector2 legPos = GetOffsetAnchor(Player) + new Vector2(0, 14 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.3f;
            Vector2 legGrav = new Vector2(-Player.direction * 0.01f, -0.1f * Player.gravDir);

            ChaosMatterParticle particle = ChaosMatterParticle.RequestNew(legPos, Player.velocity * 0.05f + legVel, legGrav, Main.rand.Next(25, 40), Main.rand.Next(-2, 3) * MathHelper.PiOver2, 0.5f + Main.rand.NextFloat(0.5f));
            particles.Add(particle);
            targetShader = Player.cLegs;
        }

        if (Player.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body))
        {
            Vector2 bodyVel = new Vector2(Main.rand.NextFloat(-0.7f, 0.2f) * Player.direction, -Main.rand.NextFloat(-0.2f, 0.5f) * Player.gravDir);
            Vector2 bodyPos = GetOffsetAnchor(Player) + Main.rand.NextVector2Circular(6, 10) + new Vector2(-6 * Player.direction, -6 * Player.gravDir).RotatedBy(Player.fullRotation) + Player.velocity * 0.5f;
            Vector2 bodyGrav = -Vector2.UnitY * Main.rand.NextFloat(0.05f, 0.08f) * Player.gravDir;

            ChaosMatterParticle particle = ChaosMatterParticle.RequestNew(bodyPos, -Player.velocity * Main.rand.NextFloat(0.1f) + bodyVel, bodyGrav, Main.rand.Next(25, 40), Main.rand.Next(-2, 3) * MathHelper.PiOver2, 0.9f + Main.rand.NextFloat(0.3f));
            particles.Add(particle);
            targetShader = Player.cBody;
        }

        particles.Update();
    }
}