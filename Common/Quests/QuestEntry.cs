using System;
using Terraria.Localization;

namespace BlockVanity.Common.Quests;

public sealed class QuestEntry
{
    public bool HasRewards => Reward.HasRewards;

    public bool IsCompleteOrClaimed => Completion == (QuestCompletionState.Claimed | QuestCompletionState.Completed);

    public LocalizedText Name => Language.GetOrRegister($"Mods.{nameof(BlockVanity)}.Quests.{NameKey}.Name");
    public LocalizedText Description => Language.GetOrRegister($"Mods.{nameof(BlockVanity)}.Quests.{NameKey}.Description");

    public QuestCompletionState Completion { get; set; }

    public string NameKey { get; set; }
    public IQuestEntryIcon Icon { get; set; }
    public IQuestEntryIcon Portrait { get; set; }

    public Func<bool> AvailableCondition { get; set; }
    public Func<bool> CompleteCondition { get; set; }

    public QuestRewardData Reward { get; set; }

    public int StarCount { get; set; }

    internal int id;
}
