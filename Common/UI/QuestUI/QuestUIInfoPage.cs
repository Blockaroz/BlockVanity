using System;
using System.Collections.Generic;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIInfoPage : UIElement
{
    private QuestUIRewardList _rewardList;

    private UIText _id;
    private UIText _name;
    private UIText _description;
    private IQuestEntryIcon _portrait;
    private int _stars;

    public QuestUIInfoPage()
    {
        Width = new StyleDimension(0f, 1f);
        Height = new StyleDimension(0f, 1f);
        IgnoresMouseInteraction = false;
        SetPadding(0f);

        UIPanel mainPanel = new UIPanel()
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(400f, 1f),
            OverflowHidden = true
        };
        mainPanel.BackgroundColor = new Color(73, 85, 186);
        mainPanel.BorderColor = new Color(89, 116, 213);
        mainPanel.SetPadding(0f);
        Append(mainPanel);

        UIHorizontalSeparator separator = new UIHorizontalSeparator()
        {
            Width = new StyleDimension(0f, 1f),
            Top = new StyleDimension(36f, 0f)
        };
        separator.Color = new Color(89, 116, 213);
        mainPanel.Append(separator);

        UIText nameText = _name = new UIText("", 0.5f, true);
        nameText.DynamicallyScaleDownToWidth = true;
        nameText.Width.Set(0f, 1f);
        nameText.Height.Set(24f, 0f);
        nameText.Top.Set(12f, 0f);
        mainPanel.Append(nameText);

        UIText idText = _id = new UIText("");
        idText.Top.Set(10, 0f);
        idText.Left.Set(18, 0f);
        idText.TextColor = Colors.RarityTrash;
        mainPanel.Append(idText);


        UIText description = _description = new UIText("", 0.5f, true);


        UIPanel border = new UIPanel()
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(0f, 1f)
        };
        border.BackgroundColor = Color.Transparent;
        border.BorderColor = new Color(89, 116, 213);
        border.SetPadding(0f);
        mainPanel.Append(border);
    }

    public void OpenEntry(QuestEntry entry)
    {
        _id?.SetText($"#{entry.id + 1}");
        _name?.SetText(entry.Name.Value);
        _description?.SetText(entry.Description.Value);
        _portrait = entry.Portrait ?? entry.Icon;
        _stars = entry.StarCount;
    }

    public void Close()
    {
        _id.SetText("");
        _name.SetText("");
        _description.SetText("");
        _portrait = null;
        _stars = 0;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
    }
}
