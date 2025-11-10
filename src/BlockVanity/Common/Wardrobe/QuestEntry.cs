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


public readonly record struct QuestReward(int Value, params int[] Items);

public delegate bool QuestCondition(Player player);

public record struct QuestEntry(
    Mod mod,
    string Name,
    int Stars,
    IWardrobePortrait SmallPortrait,
    IWardrobePortrait LargePortrait,
    QuestCondition Unlock,
    QuestCondition Complete)
{
    public LocalizedText DisplayName { get; } = mod.GetLocalization($"MagicalWardrobe.{Name}.DisplayName");

    public LocalizedText Description { get; } = mod.GetLocalization($"MagicalWardrobe.{Name}.Description");

    public QuestUnlockState CompletionState { get; set; }

    public bool Locked => CompletionState == QuestUnlockState.Locked;

    public bool Completed => CompletionState is QuestUnlockState.Complete or QuestUnlockState.Claimed;
}
