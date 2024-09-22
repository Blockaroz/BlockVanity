using System;
using Terraria.Localization;

namespace BlockVanity.Common.Quests;

public abstract class QuestEntry
{
    public bool IsCompleteOrClaimed => Completion == (QuestCompletionState.Claimed | QuestCompletionState.Completed);

    public LocalizedText Title => Language.GetOrRegister($"Mods.{nameof(BlockVanity)}.Quests.{Name}.Name");
    public LocalizedText Description => Language.GetOrRegister($"Mods.{nameof(BlockVanity)}.Quests.{Name}.Description");

    public QuestCompletionState Completion { get; set; }

    public virtual string Name => GetType().Name.Replace("Quest", "");

    public IQuestPicture Portrait { get; private set; }
    public IQuestPicture Icon { get; private set; }

    public abstract QuestRewardData Reward { get; }
    public abstract int StarCount { get; }

    public virtual bool IsAvailable() => true;
    public abstract bool IsComplete();

    public abstract IQuestPicture GetPortrait();
    public virtual IQuestPicture GetIcon() => GetPortrait();

    internal int id;

    internal void SetData(int i)
    {
        id = i;
        Portrait = GetPortrait();
        Icon = GetIcon();
        _ = Title.Value;
        _ = Description.Value;
    }
}
