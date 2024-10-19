using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;
using BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing;

public class BoneKingSkull : VanityItem
{
    public override void Load()
    {
        if (Main.dedServ)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this, equipTexture: new OversizedHeadEquipTexture(0, -4));
    }

    public BoneKingSkull() : base(ModContent.RarityType<VanityRareCommon>(), 30, 34) { }

    public override void SetStaticDefaults()
    {
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        BlockVanity.RegisterAlternateStyles(Type, ModContent.ItemType<PlatinumBoneKingSkull>(), ModContent.ItemType<HellforgedKingSkull>(), ModContent.ItemType<FrostbuiltKingSkull>());
    }
    
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
    }
}
