using System;
using System.Collections.Generic;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIInfoPage : UIElement
{
    private QuestUIRewardList _rewardList;
    private QuestUIPortrait _portrait;

    private UIList _elementList;
    private UIText _id;
    private UIText _name;

    private UITextPanel<string> _description;
    private UIPanel _descriptionPanel;
    private UIImageButton _expandButton;

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
        mainPanel.BackgroundColor = new Color(73, 90, 180);
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

        UIText idText = _id = new UIText("");
        idText.Top.Set(10f, 0f);
        idText.Left.Set(6f, 0f);
        idText.TextColor = Colors.RarityTrash;
        idText.TextOriginX = 0f;
        mainPanel.Append(idText);

        UIText nameText = _name = new UIText("", 0.5f, true);
        nameText.DynamicallyScaleDownToWidth = true;
        nameText.HAlign = 0.5f;
        nameText.Width.Set(-14f, 1f);
        nameText.Height.Set(24f, 0f);
        nameText.Top.Set(12f, 0f);
        nameText.TextOriginX = 0.5f;

        mainPanel.Append(nameText);

        UIList list = _elementList = new UIList();
        list.Top.Set(40f, 0f);
        list.Height.Set(-6f, 1f);

        QuestUIPortrait portrait = _portrait = new QuestUIPortrait();
        list.Add(portrait);

        UITextPanel<string> descriptPanel = _description = new UITextPanel<string>("");
        descriptPanel.BackgroundColor = Color.Black * 0.4f;
        descriptPanel.BorderColor = Color.White * 0.4f;
        list.Add(descriptPanel);

        mainPanel.Append(list);

        UIPanel border = new UIPanel()
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(0f, 1f)
        };
        border.BackgroundColor = Color.Transparent;
        border.BorderColor = new Color(89, 116, 213);
        border.SetPadding(0f);
        mainPanel.Append(border);

        Close();
    }

    public void OpenEntry(QuestEntry entry)
    {
        if (entry == null)
        {
            Close();
            return;
        }

        if (entry.Completion != QuestCompletionState.Hidden)
        {
            _name?.SetText(entry.Title.Value);
            _id?.SetText($"#{entry.id + 1}");
            _description?.SetText(entry.Description.Value);
            _portrait.SetEntry(entry);
        }
        else
            Close();
    }

    public void Close()
    {
        _name.SetText("???");
        _id.SetText("#-");

        _description.SetText("");
        _portrait.SetEntry(null);
    }
}
