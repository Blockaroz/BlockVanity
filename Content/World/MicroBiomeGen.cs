using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BlockVanity.Content.World;

public class MicroBiomeGen : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int index = tasks.FindIndex(n => n.Name == "Microbiomes");
        tasks.Insert(index, new PassLegacy("Vanity Microbiomes", GenerateBiomes));
    }

    public void GenerateBiomes(GenerationProgress progress, GameConfiguration config)
    {
        progress.Message = "Small additions";

        //GenerateHauntedCandles();
    }

    public static void GenerateHauntedCandles()
    {
        int candleCount = 3;

        if (Main.drunkWorld)
            candleCount = 40;
        if (Main.remixWorld)
            candleCount *= 2;

        dungeonTiles = new List<ushort>();
        for (int i = 0; i < Main.tileDungeon.Length; i++)
        {
            if (Main.tileDungeon[i])
                dungeonTiles.Add((ushort)i);
        }        

        for (int i = 0; i < candleCount; i++)
            GenerateHauntedCandle();
    }

    private static List<ushort> dungeonTiles;

    public static void GenerateHauntedCandle()
    {
        GenSearch search = Searches.Chain(new Searches.Rectangle(2000, 10000), new Conditions.IsTile(dungeonTiles.ToArray()));
        if (WorldUtils.Find(new Point(GenVars.dungeonX, GenVars.dungeonY), search, out Point candlePoint))
            WorldGen.PlaceTile(candlePoint.X, candlePoint.Y - 1, ModContent.TileType<HauntedCandleTile>(), true);
    }
}
