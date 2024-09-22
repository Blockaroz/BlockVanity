using System;
using Terraria;
using Terraria.ID;

namespace BlockVanity.Common.Quests;

public struct QuestRewardData
{
    public static QuestRewardData Empty = new QuestRewardData([], -1);

    public int[] Rewards { get; }

    public int Price { get; }
    public bool SkipFreeClaim { get; }
    public bool Reclaimable => Price > -1;

    public bool HasRewards => Rewards != null && Rewards.Length > 0;

    public QuestRewardData(int[] rewards, int reclaimPrice, bool skipFreeClaim = false)
    {
        Rewards = rewards;
        Price = reclaimPrice;
        SkipFreeClaim = skipFreeClaim;

        if (Price > -1)
        {
            int rewardValue = 0;
            foreach (int itemType in Rewards)
                rewardValue += ContentSamples.ItemsByType[itemType].value;

            Price = Math.Max(Price, rewardValue);
        }
    }

    public void Claim(Player player, bool free = false)
    {
        if (free && !SkipFreeClaim)
        {
            GiveItems(player);
            return;
        }

        bool canBuy = player.BuyItem(Price);

        if (canBuy)
            GiveItems(player);
    }

    private void GiveItems(Player player)
    {
        for (int i = 0; i < Rewards.Length; i++)
            player.QuickSpawnItem(player.GetSource_DropAsItem(), Rewards[i], 1);
    }
}
