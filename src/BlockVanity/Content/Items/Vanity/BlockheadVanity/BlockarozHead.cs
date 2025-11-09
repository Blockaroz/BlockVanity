using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BlockheadVanity;

[AutoloadEquip(EquipType.Head)]
public class BlockarozHead : VanityItem
{
    public BlockarozHead() : base(ItemRarityID.LightPurple) { }

    public override void SetStaticDefaults()
    {
        BlockheadHeadLayer.AddHead(this);
        NeckPlayerLayer.AddNeck(this);
        MiscEffectPlayer.hideHead.Add(Item.headSlot);
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
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