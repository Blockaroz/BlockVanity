using System;
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
        On_Player.PlayerFrame += SlowLegs;
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
        {
            self.armorEffectDrawOutlinesForbidden = true;
        }
        else
        {
            orig(self, drawPlayer);
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (enabled)
        {
            if (drawInfo.shadow > 0f)
            {
                a = 0.2f;
            }
        }
    }

    internal float walkCounter;
    internal int walkFrame;

    private void SlowLegs(On_Player.orig_PlayerFrame orig, Player self)
    {
        orig(self);

        if (self.legs == EquipLoader.GetEquipSlot(Mod, nameof(Excellence), EquipType.Legs))
        {
            ExcellencePlayer excellencePlayer = self.GetModPlayer<ExcellencePlayer>();

            if (self.velocity.Y == 0)
            {
                if (!Main.gameInactive)
                {
                    excellencePlayer.walkCounter += Math.Abs(self.velocity.X * 0.6f);
                }

                while (excellencePlayer.walkCounter > 8)
                {
                    excellencePlayer.walkCounter -= 8;
                    excellencePlayer.walkFrame += self.legFrame.Height;
                }

                if (excellencePlayer.walkFrame < self.legFrame.Height * 7)
                {
                    excellencePlayer.walkFrame = self.legFrame.Height * 19;
                }
                else if (excellencePlayer.walkFrame > self.legFrame.Height * 19)
                {
                    excellencePlayer.walkFrame = self.legFrame.Height * 7;
                }

                if (self.velocity.X != 0)
                {
                    self.bodyFrameCounter = 0.0;
                    self.legFrameCounter = 0.0;
                    self.legFrame.Y = excellencePlayer.walkFrame;
                }
                else
                {
                    excellencePlayer.walkCounter = 0;
                    excellencePlayer.walkFrame = 0;
                }
            }
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}
