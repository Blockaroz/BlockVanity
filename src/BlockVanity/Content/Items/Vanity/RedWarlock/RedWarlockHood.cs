using BlockVanity.Core;
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