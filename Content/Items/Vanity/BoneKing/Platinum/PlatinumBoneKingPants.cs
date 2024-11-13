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

[AutoloadEquip(EquipType.Legs)]
public class PlatinumBoneKingPants : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingPants>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingPants>().Tooltip;

    public PlatinumBoneKingPants() : base(ModContent.RarityType<VanityRareCommon>(), 30, 18) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
    }
}
