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
    public bool headAltFrame;

    public override void Load()
    {
        On_Player.PlayerFrame += SlowDownAnimation;
        On_Player.UpdateVisibleAccessory += EnableExcellence;
        On_PlayerDrawLayers.DrawPlayer_21_Head += HideHead;
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegs;
        //Torso uses normal frames
    }

    private void SlowDownAnimation(On_Player.orig_PlayerFrame orig, Player self)
    {
        orig(self);

        //int bodyFrame = self.bodyFrame.Y / self.bodyFrame.Height;
        //bool headBounce = bodyFrame == 10 || bodyFrame == 11 || bodyFrame == 16 || bodyFrame == 17;
        self.GetModPlayer<ExcellencePlayer>().headAltFrame = self.velocity.Y * self.gravDir > 0;

        if (self.GetModPlayer<ExcellencePlayer>().enabled)
        {
            //jank, but works flawlessly
            if (self.swimTime > 0)
                self.legFrameCounter -= 0.2;
            else if (self.velocity.X != 0)
                self.legFrameCounter -= (double)Math.Abs(self.velocity.X) * 0.2;

            self.armorEffectDrawOutlinesForbidden = true;
        }
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

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (enabled)
            modifiers.DisableSound();
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (enabled)
        {

        }
    }

    public override void ResetEffects()
    {
        if (Player.velocity.X == 0)
            headAltFrame = false;

        enabled = false;
    }
}
