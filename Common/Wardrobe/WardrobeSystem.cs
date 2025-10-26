using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace BlockVanity.Common.Wardrobe;

public sealed class WardrobeSystem : ModSystem
{
    public static bool IsOpen { get; private set; }

    public static void ToggleWardrobe()
    {
        IsOpen = !IsOpen;
        Main.mapFullscreen = false;
        Main.CreativeMenu.CloseMenu();
        Main.ClosePlayerChat();
    }

    // public static ModKeybind wardrobeTestKey;

    public static UserInterface WardrobeInterface;
    public static WardrobeUI WardrobeUI;

    public override void PostSetupContent()
    {
        WardrobeInterface = new UserInterface();
        WardrobeUI = new WardrobeUI();
        WardrobeInterface.SetState(WardrobeUI);

        // wardrobeTestKey = KeybindLoader.RegisterKeybind(Mod, "wardrobe test", Microsoft.Xna.Framework.Input.Keys.K);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (IsOpen && !Main.gameMenu && !Main.gameInactive)
        {
            Main.hidePlayerCraftingMenu = true;
            WardrobeInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(n => n.Name == "Vanilla: Inventory");
        layers.Insert(index, new LegacyGameInterfaceLayer($"{nameof(BlockVanity)}: Wardrobe", () =>
        {


            return true;

        }, InterfaceScaleType.UI));
    }
}