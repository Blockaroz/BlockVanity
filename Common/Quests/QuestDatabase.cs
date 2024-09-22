using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace BlockVanity.Common.Quests;

public class QuestDatabase
{
    internal List<QuestEntry> _entries = new List<QuestEntry>();

    public List<QuestEntry> Entries => _entries;

    // Set once and never again
    public int TotalStarCount { get; internal set; }

    // Set when changed
    public int CurrentStarCount { get; internal set; }

    public int UpdateCurrentStarCount()
    {
        int starCount = 0;
        foreach (QuestEntry entry in Entries)
        {
            if (entry.IsCompleteOrClaimed)
                starCount += entry.StarCount;
        }

        return starCount;
    }

    public QuestEntry Add(QuestEntry entry)
    {
        _entries.Add(entry);
        return entry;
    }

    public QuestEntry Insert(QuestEntry entry, int insertIndex)
    {
        _entries.Insert(insertIndex, entry);
        return entry;
    }

    internal void Clear()
    {
        _entries ??= new List<QuestEntry>();
        _entries.Clear();
    }

    public void CheckQuests()
    {
        foreach (QuestEntry quest in _entries.Where(n => !n.IsCompleteOrClaimed))
        {
            if (quest.Completion == QuestCompletionState.Hidden)
            {
                if (quest.IsAvailable())
                    quest.Completion = QuestCompletionState.Available;
            }
            else if (quest.Completion == QuestCompletionState.Available)
            {
                bool unlocked = quest.IsComplete();

                if (quest.Reward.HasRewards)
                    quest.Completion = QuestCompletionState.Completed;
                else
                    quest.Completion = QuestCompletionState.Claimed;
            }
        }
    }
}
