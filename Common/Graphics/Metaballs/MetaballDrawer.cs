using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Common.Graphics.Metaballs;

public abstract class MetaballDrawer : ILoadable
{
    public MetaballRenderTargetContent content;

    public void Load(Mod mod)
    {
        content = new MetaballRenderTargetContent();
        Initialize();
        Main.ContentThatNeedsRenderTargets.Add(content);
    }

    public virtual void Initialize() { }

    public void Unload()
    {
    }
}
