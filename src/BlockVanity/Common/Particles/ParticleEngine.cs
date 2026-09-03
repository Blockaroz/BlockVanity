using Daybreak.Hooks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Common.Particles;

/// <summary>
///     Contains particle systems.
/// </summary>
public static class ParticleEngine
{
    /// <summary>
    ///     Renders behind dust.
    /// </summary>
    public static ParticleRenderer Particles { get; } = new();

    /// <summary>
    ///     Renders behind front gore.
    /// </summary>
    public static ParticleRenderer GoreLayer { get; } = new();

    /// <summary>
    ///     Renders over tiles.
    /// </summary>
    public static ParticleRenderer TileLayer { get; } = new();

    internal static ParticleRenderer InterfaceLayer { get; } = new();

    [OnLoad]
    public static void Load()
    {
        if (Main.dedServ)
            return;

        On_Main.DrawDust += DrawParticles;
        On_Main.DrawGore += DrawGoreParticles;
        On_Main.DoDraw_Tiles_Solid += DrawTileParticles;
        On_Main.DrawInterface_33_MouseText += DrawInterfaceParticles;
        On_Main.UpdateParticleSystems_World += UpdateWorldParticles;
        On_Main.UpdateParticleSystems_UI += UpdateInterfaceParticles;
        IL_Main.DoUpdateInWorld += _ => { };
    }

    private static void UpdateInterfaceParticles(On_Main.orig_UpdateParticleSystems_UI orig, Main self)
    {
        orig(self);

        InterfaceLayer.Update();
    }

    private static void UpdateWorldParticles(On_Main.orig_UpdateParticleSystems_World orig, Main self)
    {
        orig(self);

        Particles.Update();
        GoreLayer.Update();
        TileLayer.Update();
    }

    private static void DrawParticles(On_Main.orig_DrawDust orig, Main self)
    {
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        Particles.Settings.AnchorPosition = -Main.screenPosition;
        Particles.Draw(Main.spriteBatch);
        Main.spriteBatch.End();

        orig(self);
    }

    private static void DrawGoreParticles(On_Main.orig_DrawGore orig, Main self)
    {
        //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        GoreLayer.Settings.AnchorPosition = -Main.screenPosition;
        GoreLayer.Draw(Main.spriteBatch);
        //Main.spriteBatch.End();

        orig(self);
    }

    private static void DrawTileParticles(On_Main.orig_DoDraw_Tiles_Solid orig, Main self)
    {
        orig(self);

        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        TileLayer.Settings.AnchorPosition = -Main.screenPosition;
        TileLayer.Draw(Main.spriteBatch);
        Main.spriteBatch.End();
    }

    private static void DrawInterfaceParticles(On_Main.orig_DrawInterface_33_MouseText orig, Main self)
    {
        //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
        // InterfaceLayer.Settings.AnchorPosition = Vector2.Zero;
        InterfaceLayer.Draw(Main.spriteBatch);
        //Main.spriteBatch.End();

        orig(self);
    }
}
