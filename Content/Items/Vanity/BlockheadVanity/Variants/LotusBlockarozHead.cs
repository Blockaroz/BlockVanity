using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BlockheadVanity.Variants;

[AutoloadEquip(EquipType.Head)]
public class LotusBlockarozHead : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BlockarozHead>().DisplayName;

    public LotusBlockarozHead() : base(ItemRarityID.Cyan) { }

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
            .AddIngredient<BlockarozHead>()
            .AddIngredient<LotusHairpin>()
            .Register();
    }
}