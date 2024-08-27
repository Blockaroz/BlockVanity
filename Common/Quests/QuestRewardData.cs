using Terraria;
using Terraria.ID;

namespace BlockVanity.Common.Quests;

public struct QuestRewardData
{
    public int[] Rewards { get; }

    public int Price { get; }
    public bool Reclaimable => Price > -1;

    public bool HasRewards => Rewards != null && Rewards.Length > 0;

    public QuestRewardData(int[] rewards, int reclaimPrice = 0)
    {
        Rewards = rewards;
        Price = reclaimPrice;

        if (Price > -1)
        {
            int priceOnTop = 0;
            foreach (int itemType in Rewards)
                priceOnTop += ContentSamples.ItemsByType[itemType].value;

            Price += priceOnTop;
        }
    }

    public void Claim(Player player, bool free = false)
    {
        if (free)
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
