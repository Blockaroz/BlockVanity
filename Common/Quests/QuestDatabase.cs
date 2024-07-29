using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace BlockVanity.Common.Quests;

public class QuestDatabase
{
    private List<QuestEntry> _entries = new List<QuestEntry>();

    public List<QuestEntry> Entries => _entries;

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

    internal void Clear() => _entries.Clear();

    public void CheckQuests()
    {
        foreach (QuestEntry quest in _entries.Where(n => !n.IsDone))
        {
            if (quest.Completion == QuestCompletionState.Hidden)
            {
                if (quest.ShowCondition?.Invoke() ?? true)
                    quest.Completion = QuestCompletionState.NotDone;
            }
            else if (quest.Completion == QuestCompletionState.NotDone)
            {
                bool unlocked = quest.UnlockCondition.Invoke();

                if (quest.HasRewards)
                    quest.Completion = QuestCompletionState.Completed;
                else
                    quest.Completion = QuestCompletionState.Claimed;
            }
        }
    }
}
