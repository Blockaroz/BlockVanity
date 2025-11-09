using BlockVanity.Content.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.TestItems;

public class PlumeriaPlantPlacer : ModItem
{
    public override string Texture => $"{nameof(BlockVanity)}/Assets/Textures/Tiles/PlumeriaPlant";

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<PlumeriaPlant>());
    }
}