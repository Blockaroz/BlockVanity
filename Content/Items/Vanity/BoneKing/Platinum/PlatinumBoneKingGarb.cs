using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;
using BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Platinum;

[AutoloadEquip(EquipType.Body)]
public class PlatinumBoneKingGarb : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingGarb>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingGarb>().Tooltip;

    public PlatinumBoneKingGarb() : base(ModContent.RarityType<VanityRareCommon>(), 34, 32) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
        BlockVanity.RegisterAlternateStyles(Type);
    }
}
