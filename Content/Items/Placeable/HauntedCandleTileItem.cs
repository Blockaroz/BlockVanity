using BlockVanity.Content.Pets.HauntedCandelabraPet;
using BlockVanity.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Placeable;

public class HauntedCandleTileItem : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<HauntedCandleTile>());
        Item.rare = ItemRarityID.Orange;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<HauntedCandle>()
            .AddCondition(Condition.InGraveyard)
            .Register();
    }

}
