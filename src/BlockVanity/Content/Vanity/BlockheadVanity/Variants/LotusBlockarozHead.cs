using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Content.Vanity;
using BlockVanity.Content.Vanity.BlockheadVanity;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.BlockheadVanity.Variants;

[AutoloadEquip(EquipType.Head)]
public class LotusBlockarozHead : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BlockarozHead>().DisplayName;

    public override int Rarity => ItemRarityID.Cyan;

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
            .AddIngredient<BlockarozHead>()
            .AddIngredient<LotusHairpin>()
            .Register();
    }
}