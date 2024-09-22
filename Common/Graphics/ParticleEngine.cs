using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Common.Graphics;

public static class ParticleEngine
{
    public static ParticleSystem particles;

    public static void Load()
    {
        particles = new ParticleSystem();

        On_Main.DrawDust += DrawParticleSystems;
        On_Main.UpdateParticleSystems += UpdateParticleSystems;
    }

    private static void UpdateParticleSystems(On_Main.orig_UpdateParticleSystems orig, Main self)
    {
        orig(self);

        if (Main.gameMenu)
            particles.Init();
        else
            particles.Update();
    }

    private static void DrawParticleSystems(On_Main.orig_DrawDust orig, Main self)
    {
        orig(self);

        particles.Draw(Main.spriteBatch, Main.screenPosition, BlendState.AlphaBlend, Main.Transform);
    }
}
