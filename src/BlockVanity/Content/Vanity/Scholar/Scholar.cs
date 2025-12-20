using BlockVanity.Common;
using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Scholar;

[AutoloadEquip(EquipType.Head)]
public class ScholarHood : VanityItem
{
    public override int Rarity => ModContent.RarityType<ToughVanityRarity>();

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
    public override int Rarity => ModContent.RarityType<ToughVanityRarity>();

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