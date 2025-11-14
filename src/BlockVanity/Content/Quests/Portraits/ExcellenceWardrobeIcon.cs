using BlockVanity.Common.Wardrobe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockVanity.Content.Quests.Portraits;

public sealed class ExcellenceWardrobeIcon : IWardrobeIcon
{
    public static readonly LazyAsset<Texture2D> Jexture = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/TheJester");

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
    }
}

public sealed class ExcellenceWardrobePortrait : IWardrobeIcon
{
    public ExcellenceWardrobePortrait()
    {

    }

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
    }
}
