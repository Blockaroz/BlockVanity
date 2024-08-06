using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;

namespace BlockVanity.Common.Quests;

public interface IQuestSortStep : IEntrySortStep<QuestEntry>, IComparer<QuestEntry>
{
    public bool Hidden { get; }
}
