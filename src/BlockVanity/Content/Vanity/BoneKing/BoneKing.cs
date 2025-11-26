using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.BoneKing;

public class BoneKingSkull : VanityItem
{
    public override void Load()
    {
        if (Main.dedServ)
        {
            return;
        }

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this, equipTexture: new OversizedHeadEquipTexture(0, -4));
    }

    public BoneKingSkull() : base(ModContent.RarityType<RarityCommonVanity>(), 30, 34) { }

    public override void SetStaticDefaults()
    {
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        BlockVanity.AddStyles(Type, ModContent.ItemType<PlatinumBoneKingSkull>());
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
    }
}

[AutoloadEquip(EquipType.Body)]
public class BoneKingGarb : VanityItem
{
    public BoneKingGarb() : base(ModContent.RarityType<RarityCommonVanity>(), 34, 32) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
        BlockVanity.AddStyles(Type, ModContent.ItemType<PlatinumBoneKingGarb>());
    }
}

[AutoloadEquip(EquipType.Legs)]
public class BoneKingPants : VanityItem
{
    public BoneKingPants() : base(ModContent.RarityType<RarityCommonVanity>(), 30, 18) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
        BlockVanity.AddStyles(Type, ModContent.ItemType<PlatinumBoneKingPants>());
    }
}