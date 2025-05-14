using BlockVanity.Content.Items.Vanity.Excellence;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Systems.Players;

public class ExcellencePlayer : ModPlayer
{
    public bool enabled;

    public override void Load()
    {
        On_Player.UpdateVisibleAccessory += EnableExcellence;
        On_Player.SetArmorEffectVisuals += ExcellenceShadows;
    }

    public override void Unload()
    {
        On_Player.UpdateVisibleAccessory -= EnableExcellence;
        On_Player.SetArmorEffectVisuals -= ExcellenceShadows;
    }

    private void EnableExcellence(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
    {
        orig(self, itemSlot, item, modded);

        if (item.ModItem is Excellence excellence)
        {
            self.GetModPlayer<ExcellencePlayer>().enabled = true;
            self.head = EquipLoader.GetEquipSlot(Mod, excellence.Name, EquipType.Head);
            self.body = EquipLoader.GetEquipSlot(Mod, excellence.Name, EquipType.Body);
            self.legs = EquipLoader.GetEquipSlot(Mod, excellence.Name, EquipType.Legs);
        }
    }

    private void ExcellenceShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        if (self.GetModPlayer<ExcellencePlayer>().enabled)
            self.armorEffectDrawOutlinesForbidden = true;
        else
            orig(self, drawPlayer);
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (enabled && drawInfo.shadow > 0f)
        {
            a = 0.33f;
            r = 1.1f;
            b = 0.3f;
            g = 0.3f;
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}