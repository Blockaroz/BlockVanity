﻿using BlockVanity.Content.Items.Vanity.Myrtle;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MiscEffectPlayer : ModPlayer
{
    public bool accBlackEye;
    public bool blockheadSkin;

    //pets
    public bool floatingSkyLanternPet;

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_21_Head_TheFace += SetEyeBlack;
        On_PlayerDrawSet.BoringSetup_End += ChangeSkinColor;
    }

    private void SetEyeBlack(On_PlayerDrawLayers.orig_DrawPlayer_21_Head_TheFace orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.GetModPlayer<MiscEffectPlayer>().accBlackEye)
            drawinfo.colorEyeWhites = new Color(20, 20, 20);

        orig(ref drawinfo);
    }

    private void ChangeSkinColor(On_PlayerDrawSet.orig_BoringSetup_End orig, ref PlayerDrawSet self)
    {
        orig(ref self);

        if (self.drawPlayer.GetModPlayer<MiscEffectPlayer>().blockheadSkin)
        {
            self.colorBodySkin = self.drawPlayer.skinColor.ToGrayscale();
            self.colorHead = self.colorBodySkin;
        }
    }

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (!mediumCoreDeath)
        {
            if (Player.name.Equals("Myrtle", System.StringComparison.CurrentCultureIgnoreCase))
                return [
                    new Item(ModContent.ItemType<PlumeriaHairpin>()),
                    new Item(ModContent.ItemType<MyrtleDress>()),
                    new Item(ModContent.ItemType<MyrtleSandals>()),
                    new Item(ModContent.ItemType<FishFood>()),
                ];
        }

        return Enumerable.Empty<Item>();
    }

    public override void ResetEffects()
    {
        accBlackEye = false;
        blockheadSkin = false;

        floatingSkyLanternPet = false;
        
    }
}
