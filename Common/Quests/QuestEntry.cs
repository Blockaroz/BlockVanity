using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Common.Quests;

public sealed class QuestEntry
{
    public bool HasRewards => rewards != null && rewards.Length > 0;

    public bool IsDone => Completion == (QuestCompletionState.Claimed | QuestCompletionState.Completed);

    public LocalizedText Name => ModContent.GetInstance<BlockVanity>().GetLocalization($"Quests.{NameIdentifier}.Name");
    public LocalizedText RequirementText => ModContent.GetInstance<BlockVanity>().GetLocalization($"Quests.{NameIdentifier}.RequirementText");

    public QuestCompletionState Completion { get; set; }

    public string NameIdentifier { get; set; }
    public IQuestEntryIcon Icon { get; set; }

    public Func<bool> ShowCondition { get; set; }
    public Func<bool> UnlockCondition { get; set; }

    public Item[] rewards;
    public bool reclaimable;
    public int reclaimPriceAdd;
}
