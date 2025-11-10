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
    public static LazyAsset<Texture2D> Jexture = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/TheJester");

    public void DrawPortrait(SpriteBatch spriteBatch, QuestEntry entry, Vector2 center, bool isHovered)
    {
        Texture2D texture = Jexture.Value;
    }
}
