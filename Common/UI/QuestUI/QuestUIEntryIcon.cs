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
using Terraria.ID;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIEntryIcon : UIElement
{
    private QuestEntry _entry;
    private bool _isPortrait;
    private float _hoverFade;
    public bool ForceHover;

    public QuestUIEntryIcon(QuestEntry entry, bool isPortrait)
    {
        Width = new StyleDimension(0f, 1f);
        Height = new StyleDimension(0f, 1f);
        SetPadding(0f);

        _entry = entry;
        _isPortrait = isPortrait;

        IgnoresMouseInteraction = true;
        OverrideSamplerState = SamplerState.PointClamp;
        UseImmediateMode = true;
    }

    public override void Update(GameTime gameTime)
    {
        bool hovering = IsMouseHovering || ForceHover;
        _hoverFade = MathHelper.Clamp(_hoverFade + (hovering ? 0.2f : -0.1f), 0, 1f);

        CalculatedStyle dimensions = GetDimensions();
        _entry.Icon?.Update(_entry, new EntryIconDrawSettings
        {
            iconbox = dimensions.ToRectangle(),
            IsPortrait = _isPortrait,
            IsHovered = hovering
        });

        base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        CalculatedStyle dimensions = GetDimensions();
        _entry.Icon?.Draw(_entry, spriteBatch, new EntryIconDrawSettings
        {
            iconbox = dimensions.ToRectangle(),
            IsPortrait = _isPortrait,
            IsHovered = IsMouseHovering || ForceHover
        });
    }
}
