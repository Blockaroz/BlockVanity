using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIEntryGrid : UIElement
{
    private List<QuestEntry> _workingEntries;
    private MouseEvent _clickEntry;
    private int _firstEntry;
    private int _lastEntry;

    public void SetWorkingEntries(List<QuestEntry> entries) => _workingEntries = entries;

    public event Action OnUpdateGrid;

    public QuestUIEntryGrid(List<QuestEntry> workingSet, MouseEvent clickEntry)
    {
        Width = new StyleDimension(0f, 1f);
        Height = new StyleDimension(0f, 1f);
        SetPadding(0f);

        _workingEntries = workingSet;
        _clickEntry = clickEntry;

        FillInEntries();
    }

    public override void Recalculate()
    {
        base.Recalculate();
        FillInEntries();
    }

    public void FillInEntries()
    {
        RemoveAllChildren();
        _lastEntry = _workingEntries.Count;
        GetEntriesToShow(out var maxEntriesWidth, out var maxEntriesHeight, out var maxEntriesToHave);
        SetRange(0, maxEntriesToHave);

        int firstEntry = _firstEntry;
        int lastEntry = Math.Min(_lastEntry, firstEntry + maxEntriesToHave);
        List<QuestEntry> list = new List<QuestEntry>();
        for (int i = firstEntry; i < lastEntry; i++)
            list.Add(_workingEntries[i]);

        int entryIndex = 0;

        for (int j = 0; j < maxEntriesHeight; j++)
        {
            for (int k = 0; k < maxEntriesWidth; k++)
            {
                if (entryIndex >= list.Count)
                    break;

                UIElement entryButton = new QuestUIEntryGridButton(list[entryIndex]);
                entryIndex++;
                entryButton.OnLeftClick += _clickEntry;
                entryButton.VAlign = entryButton.HAlign = 0.5f;
                entryButton.Left.Set(0f, (float)k / maxEntriesWidth - 0.5f + (0.5f / maxEntriesWidth));
                entryButton.Top.Set(0f, (float)j / maxEntriesHeight - 0.5f + (0.5f / maxEntriesHeight));
                entryButton.SetSnapPoint("Entries", entryIndex, new Vector2(0.5f, 0.7f));

                Append(entryButton);
            }
        }
    }

    public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
    {
        Rectangle rectangle = GetDimensions().ToRectangle();
        maxEntriesWidth = rectangle.Width / QuestUIEntryGridButton.EntryWidth;
        maxEntriesHeight = rectangle.Height / QuestUIEntryGridButton.EntryHeight;
        int num = 0;
        maxEntriesToHave = maxEntriesWidth * maxEntriesHeight - num;
    }

    public string GetRangeText()
    {
        GetEntriesToShow(out var _, out var _, out var maxEntriesToHave);
        int atEntryIndex = _firstEntry;
        int num = Math.Min(_lastEntry, atEntryIndex + maxEntriesToHave);
        int num2 = Math.Min(atEntryIndex + 1, num);
        return $"{num2}-{num} ({_lastEntry})";
    }

    public void OffsetLibraryByPages(int howManyPages)
    {
        GetEntriesToShow(out var _, out var _, out var maxEntriesToHave);
        OffsetLibrary(howManyPages * maxEntriesToHave);
    }

    public void OffsetLibrary(int offset)
    {
        GetEntriesToShow(out var _, out var _, out var maxEntriesToHave);
        SetRange(offset, maxEntriesToHave);
        FillInEntries();
    }

    private void SetRange(int offset, int maxEntriesToHave)
    {
        _firstEntry = Utils.Clamp(_firstEntry + offset, 0, Math.Max(0, _lastEntry - maxEntriesToHave));

        OnUpdateGrid?.Invoke();
    }

    public void MakeButtonGoByOffset(UIElement element, int howManyPages)
    {
        element.OnLeftClick += delegate {
            OffsetLibraryByPages(howManyPages);
        };
    }
}
