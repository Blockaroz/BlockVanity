using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Content.Tiles;
using Terraria.ModLoader;
using Terraria;

namespace BlockVanity.Content.Items.TestItems;

public class PlumeriaPlantPlacer : ModItem
{
    public override string Texture => $"{nameof(BlockVanity)}/Assets/Textures/Tiles/PlumeriaPlant";

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<PlumeriaPlant>());
    }
}
