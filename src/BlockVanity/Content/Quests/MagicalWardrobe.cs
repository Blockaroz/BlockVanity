using BlockVanity.Common.Wardrobe;
using System.Collections.Generic;
using Terraria;

namespace BlockVanity.Content.Quests;

public static class MagicalWardrobe
{
    public static List<QuestEntry> Entries { get; } = [];

    public static int TotalStarCount { get; private set; }

    public static void Populate()
    {
        AddEntry(QuestDefinitions.Common.TheBox());
        AddEntry(QuestDefinitions.Tough.ScholarOfOld());
        AddEntry(QuestDefinitions.Special.Excellence());
    }

    private static int nextID;

    private static void AddEntry(QuestEntry entry)
    {
        entry.ID = nextID++;
        Entries.Add(entry);
    }

    public static void MarkEntryCompletion(Player player, QuestEntry entry)
    {
        if (entry.IsLocked && entry.Unlock(player))
            entry.CompletionState = QuestUnlockState.Available;

        if (!entry.IsLocked && !entry.IsCompleted && entry.Complete(player))
            entry.CompletionState = QuestUnlockState.Complete;
    }
}
