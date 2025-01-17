using BlockVanity.Content.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Placeable;

public class StylotronItem : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Stylotron>());
        Item.rare = ItemRarityID.Blue;
    }
}