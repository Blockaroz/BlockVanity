using System.Collections.Generic;
using System.Linq;
using BlockVanity.Content.Items.Dyes;
using BlockVanity.Content.Items.Vanity.Myrtle;
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
    }

    private void SetEyeBlack(On_PlayerDrawLayers.orig_DrawPlayer_21_Head_TheFace orig, ref PlayerDrawSet drawinfo)
    {
        //if (drawinfo.drawPlayer.GetModPlayer<MiscEffectPlayer>().accBlackEye)
        //    drawinfo.colorEyeWhites = new Color(20, 20, 20);

        orig(ref drawinfo);
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (blockheadSkin)
        {
            drawInfo.colorBodySkin = Player.skinColor.ToGrayscale();
            drawInfo.colorHead = drawInfo.colorBodySkin;
        }

        if (accBlackEye)
            drawInfo.colorEyeWhites = new Color(20, 20, 20);
    }

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (!mediumCoreDeath)
        {
            if (Player.name.Equals("Myrtle", System.StringComparison.CurrentCultureIgnoreCase))
                return [
                    new Item(ModContent.ItemType<FishFood>()),
                    new Item(ModContent.ItemType<SeasideHairDye>()),
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
