using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;
using BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;

[AutoloadEquip(EquipType.Legs)]
public class HellforgedKingPants : VanityItem
{
    public HellforgedKingPants() : base(ModContent.RarityType<VanityRareCommon>(), 30, 18) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
    }
}
