using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BlockVanity.Common.Wardrobe;

public sealed class MagicalWardrobeUISystem : ModSystem
{
    public static bool IsOpen { get; private set; }

    public static void ToggleWardrobe()
    {
        if (Main.LocalPlayer.chest != -1)
        {
            IsOpen = false;
            return;
        }

        IsOpen = !IsOpen;
        if (IsOpen)
        {
            Main.LocalPlayer.talkNPC = -1;
            Main.playerInventory = true;
            Main.mapFullscreen = false;
            Main.InGuideCraftMenu = false;
            Main.CreativeMenu.CloseMenu();
            Main.ClosePlayerChat();

            Main.LocalPlayer.GetMagicalWardrobe().UpdateStatus();
        }

        UISliderBase.EscapeElements();

        WardrobeUI = new();
        WardrobeInterface.SetState(WardrobeUI);

        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    public static ModKeybind wardrobeTestKey;

    public static UserInterface WardrobeInterface;
    public static MagicalWardrobeUI WardrobeUI;

    public override void PostSetupContent()
    {
        WardrobeInterface = new UserInterface();
        WardrobeUI = new MagicalWardrobeUI();
        WardrobeInterface.SetState(WardrobeUI);

        wardrobeTestKey = KeybindLoader.RegisterKeybind(Mod, "wardrobe test", Microsoft.Xna.Framework.Input.Keys.K);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (wardrobeTestKey.JustPressed)
            ToggleWardrobe();

        if (IsOpen && !Main.gameMenu)
        {
            Main.hidePlayerCraftingMenu = true;

            if (!Main.gameInactive)
                WardrobeInterface?.Update(gameTime);

            if (!Main.playerInventory || 
                Main.CreativeMenu.Enabled || 
                Main.inFancyUI ||
                Main.LocalPlayer.TalkNPC != null || 
                Main.LocalPlayer.chest != -1)
                ToggleWardrobe();
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(n => n.Name == "Vanilla: Inventory");
        layers.Insert(index, new LegacyGameInterfaceLayer($"{nameof(BlockVanity)}: Wardrobe", () =>
        {
            if (IsOpen && Main.playerInventory)
            {
                Main.hidePlayerCraftingMenu = true;
                WardrobeInterface?.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
            }

            return true;

        }, InterfaceScaleType.UI));
    }
}