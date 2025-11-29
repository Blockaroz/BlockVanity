using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;

namespace BlockVanity.Common.Wardrobe;

public readonly record struct QuestReward(int[] Items, int Value, bool Purchaseable = true, int Price = -1)
{
    public static QuestReward None { get; } = new QuestReward([], 0, false, -1);

    public static QuestReward MoneyReward(int platinum, int gold, int silver, int copper) => new QuestReward(
        [], 
        Item.buyPrice(platinum, gold, silver, copper),
        Purchaseable: false,
        Price: 0);

    public static QuestReward ItemReward(int[] items, int additionalReward = 0, int purchasePrice = -1)
    {
        int itemValues = items
            .Select(i => ContentSamples.ItemsByType[i])
            .Sum(i => i.value);

        return new QuestReward(
        items,
        0,
        Purchaseable: purchasePrice > 0,
        Price: purchasePrice > 0 ? Math.Max(itemValues, purchasePrice) : -1);
    }
}