using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Content.Tiles;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BlockVanity.Common.World;

public class MicroBiomeGens : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Micro Biomes"], GenerateMicroBiomes);
    }

    public static void GenerateMicroBiomes(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
        orig(self, progress, configuration);

        // Plumeria
        bool left = WorldGen.genRand.NextBool();
        int startX = left ? WorldGen.beachDistance : Main.maxTilesX - WorldGen.beachDistance;
        int endX = left ? 0 : Main.maxTilesX;

        bool found = false;
        for (int j = (int)(Main.worldSurface * 0.33333333); j < WorldGen.oceanLevel; j++)
        {
            if (found)
                break;

            for (int i = startX; left ? (i > endX) : (i < endX); i += left ? -1 : 1)
            {
                if (!WorldGen.InWorld(i, j) || !WorldGen.InWorld(i + 1, j))
                    continue;

                if (WorldGen.SolidTileAllowTopSlope(i, j + 1) && WorldGen.SolidTileAllowTopSlope(i + 1, j + 1) && !WorldGen.SolidOrSlopedTile(i, j) && !WorldGen.SolidOrSlopedTile(i + 1, j))
                {
                    if (Main.tile[i, j + 1].TileType == TileID.Sand && Main.tile[i + 1, j + 1].TileType == TileID.Sand && Main.tile[i, j].LiquidAmount > 0)
                    {
                        Main.tile[i, j].ClearTile();
                        Main.tile[i + 1, j].ClearTile();
                        Main.tile[i, j + 1].ResetToType(TileID.Sand);
                        Main.tile[i + 1, j + 1].ResetToType(TileID.Sand);
                        WorldGen.Place2x1(i, j, (ushort)ModContent.TileType<PlumeriaPlant>());

                        WorldGen.SquareTileFrame(i, j + 1);
                        WorldGen.SquareTileFrame(i + 1, j + 1);
                        WorldGen.SquareTileFrame(i, j);
                        WorldGen.SquareTileFrame(i + 1, j);
                        found = true;
                        break;
                    }
                }
            }
        }
    }
}
