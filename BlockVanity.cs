﻿using BlockVanity.Common.Utilities;
using ReLogic.Content.Sources;
using Terraria.ModLoader;

namespace BlockVanity;

public class BlockVanity : Mod
{
    public static Mod Instance;

    public override void Load()
    {
        Instance = this;
        AllAssets.Load();
    }

    public override IContentSource CreateDefaultContentSource()
    {
        SmartContentSource source = new SmartContentSource(base.CreateDefaultContentSource());
        source.AddDirectoryRedirect("Content", "Assets/Textures");
        source.AddDirectoryRedirect("Common", "Assets/Textures");
        return source;
    }
}
