using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Content.Vanity.Misc;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.BlockheadVanity;

[AutoloadEquip(EquipType.Head)]
public class BlockarozHeadItem : VanityItem
{
    public override string Texture => Assets.Textures.Content.Vanity.BlockheadVanity.BlockarozHead_Item.KEY;

    public override int Rarity => ItemRarityID.LightPurple;

    public override void SetStaticDefaults()
    {
        BlockheadHeadLayer.AddHead(this);
        NeckPlayerLayer.AddNeck(this);
        ArmorIDs.Head.Sets.HidesHead[Item.headSlot] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<CardboardBoxItem>()
            .AddIngredient(ItemID.Silk, 15)
            .AddIngredient(ItemID.Ectoplasm, 8)
            .AddIngredient(ItemID.DarkShard)
            .AddIngredient(ItemID.LightShard)
            .Register();
    }
}