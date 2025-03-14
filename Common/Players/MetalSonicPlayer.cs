using BlockVanity.Content.Items.Vanity.MetalSonic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MetalSonicPlayer : ModPlayer
{
    public bool enabled;
    public int cSonicShader;
    public bool IsRunning => Math.Abs(Player.velocity.X) > ((Player.maxRunSpeed + Player.accRunSpeed) / 1.75f)
    && Player.velocity.Y == 0
    && Player.wingTime >= Player.wingTimeMax;

    public override void Load()
    {
        On_Player.UpdateVisibleAccessory += EnableSonic;
        On_Player.SetArmorEffectVisuals += ArmorSetShadows;

        On_PlayerDrawLayers.DrawPlayer_21_Head += HideHead;
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegs;
    }

    private void ArmorSetShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        orig(self, drawPlayer);

        if (self.GetModPlayer<MetalSonicPlayer>().enabled)
        {
            self.armorEffectDrawShadow = true;
        }
    }

    private void HideHead(On_PlayerDrawLayers.orig_DrawPlayer_21_Head orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.head != EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Head))
        {
            orig(ref drawinfo);
        }
    }

    private void HideLegs(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.legs != EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Legs))
        {
            orig(ref drawinfo);
        }
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (enabled)
        {
            drawInfo.drawPlayer.cHead = cSonicShader;
            drawInfo.drawPlayer.cBody = cSonicShader;
            drawInfo.drawPlayer.cLegs = cSonicShader;
            drawInfo.drawPlayer.cShoe = cSonicShader;
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (enabled && drawInfo.shadow > 0f)
        {
            a = 1f - drawInfo.shadow;

            if (cSonicShader <= 0)
            {
                r = 1f;
                g = 1f;
                b = 1f;
            }
        }
    }

    private void EnableSonic(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
    {
        orig(self, itemSlot, item, modded);

        if (item.ModItem is PhantomRuby ruby)
        {
            self.GetModPlayer<MetalSonicPlayer>().cSonicShader = self.dye[itemSlot % 10].dye;
            self.head = EquipLoader.GetEquipSlot(Mod, ruby.Name, EquipType.Head);
            self.body = EquipLoader.GetEquipSlot(Mod, ruby.Name, EquipType.Body);
            self.legs = EquipLoader.GetEquipSlot(Mod, ruby.Name, EquipType.Legs);
        }
    }

    public override void ResetEffects()
    {
        cSonicShader = 0;
        enabled = false;
    }
}