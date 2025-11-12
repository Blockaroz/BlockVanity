using BlockVanity.Content.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace BlockVanity.Common.Wardrobe;

public sealed class MagicalWardrobeUI : UIState
{
    public UISearchBar SearchBar { get; }

    public UIGrid EntryGrid { get; }

    public MagicalWardrobeUI()
    {
        const float statusBarHeight = 32;

        Left.Set(20f, 0f);
        Top.Set(320f, 0f);

        int entryWidth = 74;
        int entryHeight = 82;
        float availableHeight = Main.screenHeight - Top.Pixels - statusBarHeight - 200;

        int maxCountX = 5;
        int maxCountY = 5;
        SetPadding(4f);

        MinHeight.Set(100, 0);
        MaxHeight.Set(500, 0);

        SearchBar = new UISearchBar(LocalizedText.Empty, 0.67f);
        SearchBar.Width.Set(100f, 0f);
        SearchBar.Left.Set(-SearchBar.Width.Pixels, 0f);

        EntryGrid = [];
        EntryGrid.ListPadding = 4f;
        EntryGrid.Left.Set(4, 0f);
        EntryGrid.Top.Set(statusBarHeight + 4, 0f);
        EntryGrid.Width.Set(entryWidth * maxCountX, 0f);
        EntryGrid.Height.Set(entryHeight * maxCountY, 0f);
        Append(EntryGrid);

        Width.Set(EntryGrid.Width.Pixels + 170, 0);
        Height.Set(statusBarHeight + EntryGrid.Height.Pixels + 8, 0f);
    }

    public void Populate()
    {
        for (int i = 0; i < MagicalWardrobe.Entries.Count; i++)
        {
            var entry = new WardrobeEntryButton(MagicalWardrobe.Entries[i]);
            EntryGrid.Add(entry);
        }
    }

    private bool hasPopulated;

    public override void Update(GameTime gameTime)
    {
        if (!hasPopulated)
        {
            hasPopulated = true;
            Populate();
            return;
        }

        base.Update(gameTime);

        if (IsMouseHovering)
        {
            Main.LocalPlayer.mouseInterface = true;
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        Rectangle rectangle = GetDimensions().ToRectangle();
        Utils.DrawSplicedPanel(spriteBatch, TextureAssets.InventoryBack13.Value, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 10, 10, 10, 10, new Color(50, 65, 119, 200));

        Rectangle grid = EntryGrid.GetDimensions().ToRectangle();
        Utils.DrawSplicedPanel(spriteBatch, UICommon.InnerPanelTexture.Value, grid.X - 4, grid.Y - 4, grid.Width, grid.Height, 10, 10, 10, 10, Color.White);
    }
}
