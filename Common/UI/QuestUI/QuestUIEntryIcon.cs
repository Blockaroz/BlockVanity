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
using ReLogic.Graphics;
using Terraria.GameContent;

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
        _hoverFade = MathHelper.Lerp(_hoverFade, hovering ? 1f : 0f, hovering ? 0.2f : 0.1f);
        if (_hoverFade < 0.05f)
            _hoverFade = 0f;
        if (_hoverFade > 0.99f)
            _hoverFade = 1f;

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

        if (_hoverFade > 0)
        {
            Vector2 bottomOfEntry = dimensions.Center() + new Vector2(0f, dimensions.Height * 0.35f);
            Vector2 textPosition = dimensions.Center() + new Vector2(0, dimensions.Height * (0.8f - _hoverFade * 0.45f));
            Texture2D back = AllAssets.Textures.Glow[0].Value;
            Texture2D starTexture = TextureAssets.Star[0].Value;

            spriteBatch.Draw(back, bottomOfEntry, back.Frame(), Color.Black * 0.5f * _hoverFade, 0f, back.Size() * new Vector2(0.5f, 0.35f), new Vector2(1.2f, 1f), 0, 0);
            spriteBatch.Draw(starTexture, textPosition + Vector2.UnitX * 5, starTexture.Frame(), Color.White, 0f, starTexture.Size() * new Vector2(0f, 0.5f), 1f, 0, 0);
            string countText = _entry.StarCount.ToString();
            Utils.DrawBorderString(spriteBatch, countText, textPosition, Color.White, 1f, 1f, 0.36f);

        }
    }
}
