using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.UI;
using Terraria;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIEntryIcon : UIElement
{
    private QuestEntry _entry;
    private bool _isPortrait;
    public bool ForceHover;

    public QuestUIEntryIcon(QuestEntry entry, bool isPortrait)
    {
        Width = new StyleDimension(0f, 1f);
        Height = new StyleDimension(0f, 1f);
        SetPadding(0f);

        _entry = entry;
        _isPortrait = isPortrait;

        IgnoresMouseInteraction = true;
        OverrideSamplerState = Main.DefaultSamplerState;
        UseImmediateMode = true;
    }

    public override void Update(GameTime gameTime)
    {
        CalculatedStyle dimensions = GetDimensions();
        _entry.Icon?.Update(_entry, new EntryIconDrawSettings
        {
            iconbox = dimensions.ToRectangle(),
            IsPortrait = _isPortrait,
            IsHovered = IsMouseHovering || ForceHover
        });

        base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        _entry.Icon?.Draw(_entry, spriteBatch, new EntryIconDrawSettings
        {
            iconbox = dimensions.ToRectangle(),
            IsPortrait = _isPortrait,
            IsHovered = IsMouseHovering || ForceHover
        });
    }
}
