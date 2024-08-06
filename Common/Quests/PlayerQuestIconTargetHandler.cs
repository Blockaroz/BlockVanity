using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Common.Quests;

public class PlayerQuestIconTargetHandler : ILoadable
{
    public static event Action<SpriteBatch> DrawPlayerToTarget;

    public void Load(Mod mod)
    {
        On_Main.CheckMonoliths += On_Main_CheckMonoliths;
    }

    private void On_Main_CheckMonoliths(On_Main.orig_CheckMonoliths orig)
    {
        if (!Main.hideUI && !Main.gameMenu)
        {
            DrawPlayerToTarget?.Invoke(Main.spriteBatch);

            Main.spriteBatch.GraphicsDevice.SetRenderTarget(null);
            Main.spriteBatch.GraphicsDevice.Clear(Color.Transparent);
        }

        orig();
    }

    public void Unload() { }
}
