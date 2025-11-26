using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Content.Vanity;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.BlockheadVanity;

[AutoloadEquip(EquipType.Head)]
public class BlockarozHead : VanityItem
{
    public BlockarozHead() : base(ItemRarityID.LightPurple) { }

    public override void SetStaticDefaults()
    {
        BlockheadHeadLayer.AddHead(this);
        NeckPlayerLayer.AddNeck(this);
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        BlockVanity.Sets.HideHead[Item.headSlot] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<CardboardBox>()
            .AddIngredient(ItemID.Silk, 15)
            .AddIngredient(ItemID.Ectoplasm, 8)
            .AddIngredient(ItemID.DarkShard)
            .AddIngredient(ItemID.LightShard)
            .Register();
    }
}