using System.Collections.Generic;
using Terraria.ModLoader;

namespace BlockVanity.Common.Quests;

public static class QuestSortSteps
{
    public static List<IQuestSortStep> DefaultSteps => [
        new ByQuestIDInternal(),
        new ByQuestID(),
        new ByUnlockState(),
        new ByStarCount(),
        new ByCoinValue(),
        new ByAlphabet(),
        ];

    public class ByQuestIDInternal : IQuestSortStep
    {
        public bool Hidden => true;

        public int Compare(QuestEntry x, QuestEntry y) => x.id.CompareTo(y.id);

        public string GetDisplayNameKey() => $"Mods.{nameof(BlockVanity)}.UI.Sort_QuestID";
    }

    public class ByQuestID : IQuestSortStep
    {
        public bool Hidden => false;

        public int Compare(QuestEntry x, QuestEntry y) => x.id.CompareTo(y.id);

        public string GetDisplayNameKey() => $"Mods.{nameof(BlockVanity)}.UI.Sort_QuestID";
    }

    public class ByUnlockState : IQuestSortStep
    {
        public bool Hidden => false;

        public int Compare(QuestEntry x, QuestEntry y) => x.Completion.CompareTo(y.Completion);

        public string GetDisplayNameKey() => "BestiaryInfo.Sort_Unlocks";
    }

    public class ByStarCount : IQuestSortStep
    {
        public bool Hidden => false;

        public int Compare(QuestEntry x, QuestEntry y) => x.StarCount.CompareTo(y.StarCount);

        public string GetDisplayNameKey() => $"Mods.{nameof(BlockVanity)}.UI.Sort_StarCount";
    }

    public class ByCoinValue : IQuestSortStep
    {
        public bool Hidden => false;

        public int Compare(QuestEntry x, QuestEntry y) => x.ReclaimPrice.CompareTo(y.ReclaimPrice);

        public string GetDisplayNameKey() => "BestiaryInfo.Sort_Coins";
    }

    public class ByAlphabet : IQuestSortStep
    {
        public bool Hidden => false;

        public int Compare(QuestEntry x, QuestEntry y) => x.Name.Value.CompareTo(y.Name.Value);

        public string GetDisplayNameKey() => "BestiaryInfo.Sort_Alphabetical";
    }
}
