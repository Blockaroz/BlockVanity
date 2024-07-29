using BlockVanity.Common.Graphics;
using BlockVanity.Common.Quests;
using BlockVanity.Common.Utilities;
using BlockVanity.Content;
using ReLogic.Content.Sources;
using Terraria.ModLoader;

namespace BlockVanity;

public class BlockVanity : Mod
{
    public static Mod Instance;

    public override void Load()
    {
        AllAssets.Load();
        ParticleEngine.Load();

        Instance = this;
    }

    public override IContentSource CreateDefaultContentSource()
    {
        SmartContentSource source = new SmartContentSource(base.CreateDefaultContentSource());
        source.AddDirectoryRedirect("Content", "Assets/Textures");
        source.AddDirectoryRedirect("Common", "Assets/Textures");
        return source;
    }
}
