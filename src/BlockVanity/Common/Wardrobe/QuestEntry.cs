using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Common.Wardrobe;

public enum QuestUnlockState
{
    Locked,
    Available,
    Complete,
    Claimed
}


public readonly record struct QuestReward(int Value, params int[] Items)
{
    public static QuestReward None { get; } = new QuestReward(0);
}

public delegate bool QuestCondition(Player player);

public record class QuestEntry(
    Mod Mod,
    string Name,
    int Stars,
    IWardrobePortrait SmallPortrait,
    IWardrobePortrait LargePortrait,
    QuestCondition Unlock,
    QuestCondition Complete,
    QuestReward Reward)
{
    public int ID { get; set; }

    public QuestUnlockState CompletionState { get; set; }

    public LocalizedText DisplayName { get; } = Mod.GetLocalization($"MagicalWardrobe.{Name}.DisplayName", () => Name);

    public LocalizedText Description { get; } = Mod.GetLocalization($"MagicalWardrobe.{Name}.Description", () => string.Empty);

    public bool IsLocked => CompletionState == QuestUnlockState.Locked;

    public bool IsCompleted => CompletionState is QuestUnlockState.Complete or QuestUnlockState.Claimed;
}
