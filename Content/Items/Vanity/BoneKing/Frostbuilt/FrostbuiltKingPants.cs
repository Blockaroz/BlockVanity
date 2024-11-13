using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;
using BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;

[AutoloadEquip(EquipType.Legs)]
public class FrostbuiltKingPants : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingPants>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingPants>().Tooltip;

    public FrostbuiltKingPants() : base(ModContent.RarityType<VanityRareCommon>(), 30, 18) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
    }
}
