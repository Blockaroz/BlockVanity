using BlockVanity.Common.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Scholar;

[AutoloadEquip(EquipType.Head)]
public class ScholarHood : VanityItem
{
    public ScholarHood() : base(ItemRarityID.Blue) { }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 15)
            .AddIngredient(ItemID.Diamond, 2)
            .AddTile(TileID.Loom)
            .Register();
    }
}

[AutoloadEquip(EquipType.Body)]
public class ScholarCloak : VanityItem
{
    public ScholarCloak() : base(ItemRarityID.Blue) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 15)
            .AddIngredient(ItemID.Diamond, 15)
            .AddTile(TileID.Loom)
            .Register();
    }
}
