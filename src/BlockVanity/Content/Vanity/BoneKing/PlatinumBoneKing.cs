using BlockVanity.Common;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.BoneKing;

public class PlatinumBoneKingSkull : VanityItem
{
    public override void Load()
    {
        if (Main.dedServ)
        {
            return;
        }

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this, equipTexture: new OversizedHeadEquipTexture(0, -4));
    }

    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingSkull>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingSkull>().Tooltip;

    public PlatinumBoneKingSkull() : base(ModContent.RarityType<CommonVanityRarity>(), 30, 34) { }

    public override void SetStaticDefaults()
    {
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
    }
}

[AutoloadEquip(EquipType.Body)]
public class PlatinumBoneKingGarb : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingGarb>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingGarb>().Tooltip;

    public PlatinumBoneKingGarb() : base(ModContent.RarityType<CommonVanityRarity>(), 34, 32) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
        BlockVanity.AddStyles(Type);
    }
}

[AutoloadEquip(EquipType.Legs)]
public class PlatinumBoneKingPants : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingPants>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingPants>().Tooltip;

    public PlatinumBoneKingPants() : base(ModContent.RarityType<CommonVanityRarity>(), 30, 18) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
    }
}