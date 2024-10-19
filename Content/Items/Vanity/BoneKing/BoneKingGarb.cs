using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;
using BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing;

[AutoloadEquip(EquipType.Body)]
public class BoneKingGarb : VanityItem
{
    public BoneKingGarb() : base(ModContent.RarityType<VanityRareCommon>(), 34, 32) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
        BlockVanity.RegisterAlternateStyles(Type, ModContent.ItemType<PlatinumBoneKingGarb>(), ModContent.ItemType<HellforgedKingGarb>(), ModContent.ItemType<FrostbuiltKingGarb>());
    }
}
