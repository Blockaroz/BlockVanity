using BlockVanity.Common.Wardrobe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockVanity.Content.Quests.Portraits;

public sealed class ExcellenceWardrobePortrait : IWardrobePortrait
{
    public static readonly LazyAsset<Texture2D> Jexture = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/TheJester");

    private float hoverScale = 1f;

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
        if (info.Hovering)
            hoverScale = MathHelper.Lerp(hoverScale, 2f, 0.2f);
        else
            hoverScale = MathHelper.Lerp(hoverScale, 1f, 0.2f);
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
    }
}
