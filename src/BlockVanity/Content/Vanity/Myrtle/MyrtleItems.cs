using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Common.UI;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Myrtle;

public class FishFoodItem() : VanityItem(width: 20, height: 28, accessory: true)
{
    public override int Rarity => ItemRarityID.Green;

    public override void UpdateVanity(Player player)
    {
        //HitEffectPlayer.SetSkinHitSound(player, AllAssets.Sounds.FishyHit);
        player.GetModPlayer<BlueFishSkinPlayer>().enabled = true;
        player.GetModPlayer<ReskinPlayer>().enabled = true;
    }
}

[AutoloadEquip(EquipType.Head)]
public class PlumeriaHairpinItem() : VanityItem(value: Item.buyPrice(0, 10, 0, 0))
{
    public override int Rarity => ItemRarityID.Green;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => true;
    public override bool IsVanitySet(int head, int body, int legs) => true;

    public override void UpdateArmorSet(Player player)
    {
        if (AreaEffectsToggle.IsActive(player))
            Lighting.AddLight(player.MountedCenter, Color.SlateGray.ToVector3() * 0.33f);
    }

    public override void UpdateVanitySet(Player player) => UpdateArmorSet(player);
}

[AutoloadEquip(EquipType.Body)]
public class MyrtleDressItem : VanityItem
{
    public override int Rarity => ItemRarityID.Green;
}

[AutoloadEquip(EquipType.Legs)]
public class MyrtleSandalsItem : VanityItem
{
    public override int Rarity => ItemRarityID.Green;

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}_Slim", EquipType.Legs, name: this.Name + "_Slim");
    }

    public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
    {
        if (!male)
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name + "_Slim", EquipType.Legs);
    }
}