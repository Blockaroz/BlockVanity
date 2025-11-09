using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace BlockVanity.Common.Wardrobe;

public sealed class MagicalWardrobeUI : UIState
{
    public sealed class WardrobeEntryButton : UIElement
    {
        public QuestEntry Entry { get; set; }

        public WardrobeEntryButton(QuestEntry entry)
        {
            Entry = entry;
        }
    }

    public MagicalWardrobeUI()
    {
        Left.Set(20f, 0f);
        Top.Set(340f, 0f);
        Width.Set(540, 0);
        Height.Set(500, 0);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        Rectangle rectangle = GetDimensions().ToRectangle();
        Utils.DrawSplicedPanel(spriteBatch, TextureAssets.InventoryBack.Value, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 8, 8, 8, 8, Color.White);
    }
}
