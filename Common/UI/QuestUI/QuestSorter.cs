using System;
using System.Collections.Generic;

namespace BlockVanity.Common.Quests;

public class QuestSorter : IComparer<QuestEntry>
{
    public List<IQuestSortStep> Steps = new List<IQuestSortStep>();
    public string searchString = null;
    public bool descending = false;

    public int Compare(QuestEntry x, QuestEntry y)
    {
        int result = 0;

        for (int i = 0; i < Steps.Count; i++)
        {
            result = Steps[i].Compare(x, y);
            if (result != 0)
                return result;
        }

        if (searchString != null)
            result = SearchCompare(x, y);

        if (descending) 
            return -result;

        return result;
    }

    private int SearchCompare(QuestEntry x, QuestEntry y)
    {
        bool matchesX = searchString.ToLower().Contains(x.Name.Value, StringComparison.OrdinalIgnoreCase);
        bool matchesY = searchString.ToLower().Contains(y.Name.Value, StringComparison.OrdinalIgnoreCase);
        return matchesX.CompareTo(matchesY);
    }
}
