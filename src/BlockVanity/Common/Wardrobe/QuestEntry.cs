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

public record class QuestEntry(
    Mod Mod,
    string Name,
    int Stars, 
    IWardrobePortrait Portrait,
    QuestCondition UnlockCondition,
    QuestCondition CompleteCondition)
{
    public LocalizedText DisplayName { get; } = Mod.GetLocalization($"MagicalWardrobe.{Name}.Name");

    public LocalizedText Description { get; } = Mod.GetLocalization($"MagicalWardrobe.{Name}.Description");

    public QuestUnlockState CompletionState { get; set; }

    public bool Locked => CompletionState == QuestUnlockState.Locked;

    public bool Completed => CompletionState is QuestUnlockState.Complete or QuestUnlockState.Claimed;

    public bool CheckAvailability(Player player)
    {
        if (!Locked)
            return true;

        if (UnlockCondition?.Invoke(player) ?? true)
        {
            CompletionState = QuestUnlockState.Available;
            return true;
        }

        return false;
    }

    public bool CheckCompletion(Player player)
    {
        if (Completed)
            return true;

        if (!CheckAvailability(player))
            return false;

        if (CompleteCondition?.Invoke(player) ?? true)
        {
            CompletionState = QuestUnlockState.Complete;
            return true;
        }

        return false;
    }
}
