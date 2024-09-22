using System;
using BlockVanity.Common.Players;
using BlockVanity.Content.Items.Vanity.CountChaos;
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
        On_Player.SetArmorEffectVisuals += ExcellenceShadows;
        On_Player.PlayerFrame += SlowLegs;
        On_PlayerDrawLayers.DrawPlayer_21_Head += HideHead;
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegs;
        //Torso uses normal frames
    }

    private void HideLegs(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.legs != EquipLoader.GetEquipSlot(Mod, nameof(Excellence), EquipType.Legs))
            orig(ref drawinfo);
    }

    private void HideHead(On_PlayerDrawLayers.orig_DrawPlayer_21_Head orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.head != EquipLoader.GetEquipSlot(Mod, nameof(Excellence), EquipType.Head))
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

    private void ExcellenceShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        if (self.GetModPlayer<ExcellencePlayer>().enabled)
            self.armorEffectDrawOutlinesForbidden = true;
        else
            orig(self, drawPlayer);
    }

    private void ExcellenceRunEffect(On_Player.orig_SpawnFastRunParticles orig, Player self)
    {
        if (!self.GetModPlayer<MiscEffectPlayer>().disableBootsEffect)
            orig(self);
    }

    internal float walkCounter;
    internal int walkFrame;

    private void SlowLegs(On_Player.orig_PlayerFrame orig, Player self)
    {
        orig(self);

        ExcellencePlayer excellencePlayer = self.GetModPlayer<ExcellencePlayer>();

        if (excellencePlayer.enabled)
        {
            if (self.velocity.Y == 0)
            {
                excellencePlayer.walkCounter += Math.Abs(self.velocity.X * 0.5f);

                while (excellencePlayer.walkCounter > 8)
                {
                    excellencePlayer.walkCounter -= 8;
                    excellencePlayer.walkFrame += self.legFrame.Height;
                }

                if (excellencePlayer.walkFrame < self.legFrame.Height * 7)
                    excellencePlayer.walkFrame = self.legFrame.Height * 19;
                else if (excellencePlayer.walkFrame > self.legFrame.Height * 19)
                    excellencePlayer.walkFrame = self.legFrame.Height * 7;

                if (self.velocity.X != 0)
                {
                    self.bodyFrameCounter = 0.0;
                    self.legFrameCounter = 0.0;
                    self.legFrame.Y = excellencePlayer.walkFrame;
                }

            }
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}
