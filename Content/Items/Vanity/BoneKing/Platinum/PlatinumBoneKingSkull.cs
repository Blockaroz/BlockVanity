using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;
using BlockVanity.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Platinum;

public class PlatinumBoneKingSkull : VanityItem
{
    public override void Load()
    {
        if (Main.dedServ)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this, equipTexture: new OversizedHeadEquipTexture(0, -4));
    }

    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingSkull>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingSkull>().Tooltip;

    public PlatinumBoneKingSkull() : base(ModContent.RarityType<VanityRareCommon>(), 30, 34) { }

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
