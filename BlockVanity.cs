﻿using BlockVanity.Core;
using ReLogic.Content.Sources;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace BlockVanity;

public class BlockVanity : Mod
{
    public static BlockVanity Instance => ModContent.GetInstance<BlockVanity>();

    public override void Load()
    {
        AllAssets.Load(this);
        StylesByID = new Dictionary<int, int[]>();
    }

    public override IContentSource CreateDefaultContentSource() => new AssetDirectorySource(base.CreateDefaultContentSource());

    public static Dictionary<int, int[]> StylesByID { get; private set; }

    public static void AddStyles(int originalItem, params int[] styleItems)
    {
        StylesByID.TryAdd(originalItem, styleItems);

        for (int i = 0; i < styleItems.Length; i++)
        {
            int[] stylesForMe = new int[styleItems.Length];
            stylesForMe[0] = originalItem;
            int offset = 0;
            for (int j = 0; j < stylesForMe.Length; j++)
            {
                if (i != j)
                {
                    stylesForMe[j - offset] = styleItems[j];
                }
                else
                {
                    offset++;
                }
            }

            StylesByID.TryAdd(styleItems[i], stylesForMe);
        }
    }
}