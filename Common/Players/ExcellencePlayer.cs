using System;
using BlockVanity.Content.Items.Vanity.Excellence;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Systems.Players;

public class ExcellencePlayer : ModPlayer
{
    public bool enabled;

    public override void Load()
    {
        On_Player.UpdateVisibleAccessory += EnableExcellence;
        On_PlayerDrawLayers.DrawPlayer_21_Head += HideHead;
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegs;
        //Torso uses normal frames
    }

    private void HideLegs(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.legs != EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<Excellence>().Name, EquipType.Legs))
            orig(ref drawinfo);
    }

    private void HideHead(On_PlayerDrawLayers.orig_DrawPlayer_21_Head orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.head != EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<Excellence>().Name, EquipType.Head))
            orig(ref drawinfo);
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

    public override void ResetEffects()
    {
        enabled = false;
    }
}
