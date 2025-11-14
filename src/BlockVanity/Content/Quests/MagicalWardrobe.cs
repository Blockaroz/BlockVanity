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

        for (int i = 0; i < 24; i++)
            AddEntry(new QuestEntry(BlockVanity.Instance, "Null", i, null, null, QuestReward.None));
    }

    private static int nextID;

    private static void AddEntry(QuestEntry entry)
    {
        entry.ID = nextID++;
        Entries.Add(entry);
    }
}
