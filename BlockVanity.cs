using System.Collections.Generic;
using BlockVanity.Common.Graphics;
using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Content.Sources;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity;

public class BlockVanity : Mod
{
    public static Mod Instance;

    public override void Load()
    {
        ParticleEngine.Load();
        AllAssets.Load();
        StylesByID = new Dictionary<int, int[]>();
        Instance = this;
    }

    public override IContentSource CreateDefaultContentSource()
    {
        SmartContentSource source = new SmartContentSource(base.CreateDefaultContentSource());
        source.AddDirectoryRedirect("Content", "Assets/Textures");
        source.AddDirectoryRedirect("Common", "Assets/Textures");
        return source;
    }

    public static Dictionary<int, int[]> StylesByID { get; private set; }

    public static void RegisterAlternateStyles(int originalItem, params int[] styleItems)
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
                    stylesForMe[j - offset] = styleItems[j];
                else
                    offset++;
            }

            StylesByID.TryAdd(styleItems[i], stylesForMe);
        }
    }
}
