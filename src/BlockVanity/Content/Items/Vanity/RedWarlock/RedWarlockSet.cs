using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.RedWarlock;

[AutoloadEquip(EquipType.Head)]
public class RedWarlockHood : VanityItem
{
    public RedWarlockHood() : base(ItemRarityID.Orange) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 10)
            .AddIngredient(ItemID.Bone, 2)
            .AddIngredient(ItemID.Ectoplasm, 2)
            .AddTile(TileID.Loom)
            .Register();
    }
}

[AutoloadEquip(EquipType.Body)]
public class RedWarlockRobe : VanityItem
{
    public RedWarlockRobe() : base(ItemRarityID.Orange) { }

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

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 25)
            .AddIngredient(ItemID.Ectoplasm, 4)
            .AddTile(TileID.Loom)
            .Register();
    }
}
