using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Midra;

[AutoloadEquip(EquipType.Body)]
public class SkinOfMidra : VanityItem
{
    public SkinOfMidra() : base(ItemRarityID.Cyan) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
    }

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }

    public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
    {
        robes = true;
        equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
    }
}