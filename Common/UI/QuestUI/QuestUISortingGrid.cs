using System;
using System.Collections.Generic;
using System.Reflection;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUISortingGrid : UIPanel
{
    private QuestSorter _sorter;
    private List<GroupOptionButton<int>> _buttons;
    private List<int> _selections;

    public List<int> Selections => _selections;

    public event Action OnClickingOption;

    public QuestUISortingGrid(QuestSorter sorter)
    {
        _sorter = sorter;
        _buttons = new List<GroupOptionButton<int>>();
        _selections = new List<int>();
        Width = new StyleDimension(0f, 1f);
        Height = new StyleDimension(0f, 1f);
        BackgroundColor = new Color(35, 40, 83) * 0.5f;
        BorderColor = new Color(35, 40, 83) * 0.5f;
        IgnoresMouseInteraction = false;
        SetPadding(0f);
        BuildGrid();
    }

    private void BuildGrid()
    {
        List<IQuestSortStep> defaultSteps = QuestSortSteps.DefaultSteps;

        int height = 0;
        for (int i = 0; i < defaultSteps.Count; i++)
        {
            if (!defaultSteps[i].Hidden)
                height++;
        }

        UIPanel mainPanel = new UIPanel
        {
            Width = new StyleDimension(146f, 0f),
            Height = new StyleDimension(height * 28 + 10, 0f),
            HAlign = 1f,
            VAlign = 0f,
            Top = new StyleDimension(0f, 0f),
            Left = new StyleDimension(-200, 0f)
        };

        mainPanel.BorderColor = new Color(89, 116, 213, 255) * 0.9f;
        mainPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
        mainPanel.SetPadding(0f);
        Append(mainPanel);

        UIPanel numberPanel = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale"), null, 6, 32)
        {
            Width = new StyleDimension(20f, 0f),
            Height = new StyleDimension(height * 28 - 2, 0f),
            Left = new StyleDimension(4, 0f),
            VAlign = 0.5f,
            Top = new StyleDimension(0f, 0f)
        };

        numberPanel.BorderColor = Color.Transparent;
        numberPanel.BackgroundColor = Colors.InventoryDefaultColor * 0.7f;
        mainPanel.Append(numberPanel);

        int stepIndex = 0;
        for (int j = 0; j < defaultSteps.Count; j++)
        {
            IQuestSortStep sortStep = defaultSteps[j];
            if (!sortStep.Hidden)
            {
                GroupOptionButton<int> groupOptionButton = new GroupOptionButton<int>(j, Language.GetOrRegister(sortStep.GetDisplayNameKey()), null, Color.White, null, 0.8f)
                {
                    Width = new StyleDimension(114f, 0f),
                    Height = new StyleDimension(26, 0f),
                    Left = new StyleDimension(-6, 0f),
                    HAlign = 1f,
                    Top = new StyleDimension(6 + 28 * stepIndex, 0f)
                };

                groupOptionButton.ShowHighlightWhenSelected = false;
                groupOptionButton.OnLeftClick += ClickOption;
                groupOptionButton.SetSnapPoint("SortSteps", stepIndex);
                mainPanel.Append(groupOptionButton);
                _buttons.Add(groupOptionButton);
                stepIndex++;
            }
        }

        foreach (GroupOptionButton<int> item in _buttons)
            item.SetCurrentOption(-1);
    }

    private void ClickOption(UIMouseEvent evt, UIElement listeningElement)
    {
        GroupOptionButton<int> item = (GroupOptionButton<int>)listeningElement;

        item.SetCurrentOption(_selections.Count);

        if (!_selections.Contains(item.OptionValue))
        {
            item.SetColor(new Color(152, 175, 235), 1f);
            _selections.Add(item.OptionValue);
        }
        else
        {
            item.SetColor(Colors.InventoryDefaultColor, 0.7f);
            _selections.Remove(item.OptionValue);
        }

        _sorter.Steps.Clear();

        for (int i = 0; i < QuestSortSteps.DefaultSteps.Count; i++)
        {
            if (QuestSortSteps.DefaultSteps[i].Hidden)
                _sorter.Steps.Add(QuestSortSteps.DefaultSteps[i]);
        }

        for (int i = 0; i < Selections.Count; i++)
            _sorter.Steps.Add(QuestSortSteps.DefaultSteps[Selections[i]]);

        OnClickingOption?.Invoke();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (GroupOptionButton<int> item in _buttons)
        {
            Vector2 indexPos = new Vector2(item.GetDimensions().X - 13f, item.GetDimensions().Y + item.GetDimensions().Height / 2f);
            int index = _selections.IndexOf(item.OptionValue);

            if (index >= 0)
                Utils.DrawBorderString(spriteBatch, (index + 1).ToString(), indexPos, Color.White, 1f, 0.4f, 0.4f);
            else
                Utils.DrawBorderString(spriteBatch, "#", indexPos, Colors.InventoryDefaultColor * 0.9f, 1.1f, 0.4f, 0.4f);
        }
    }

    public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
    {
        maxEntriesWidth = 1;
        maxEntriesHeight = _buttons.Count;
        maxEntriesToHave = _buttons.Count;
    }
}
