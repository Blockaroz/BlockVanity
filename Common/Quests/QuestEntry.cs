using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Common.Quests;

public sealed class QuestEntry
{
    public bool HasRewards => Rewards != null && Rewards.Length > 0;

    public bool IsCompleteOrClaimed => Completion == (QuestCompletionState.Claimed | QuestCompletionState.Completed);

    public LocalizedText Name => Language.GetOrRegister($"Mods.{nameof(BlockVanity)}.Quests.{NameKey}.Name");
    public LocalizedText Description => Language.GetOrRegister($"Mods.{nameof(BlockVanity)}.Quests.{NameKey}.Description");

    public QuestCompletionState Completion { get; set; }

    public string NameKey { get; set; }
    public IQuestEntryIcon Icon { get; set; }
    public IQuestEntryIcon Portrait { get; set; }

    public Func<bool> AvailableCondition { get; set; }
    public Func<bool> CompleteCondition { get; set; }

    public int[] Rewards { get; set; }
    public bool Reclaimable { get; set; }
    private int reclaimPrice;
    public int ReclaimPrice
    {
        get => reclaimPrice;
        set
        {
            int additional = 0;
            foreach (int itemType in Rewards)
                additional += ContentSamples.ItemsByType[itemType].value;

            reclaimPrice = value + additional;
        }
    }

    public int StarCount { get; set; }

    internal int id;
}
