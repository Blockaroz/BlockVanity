using BlockVanity.Common.Quests;
using BlockVanity.Content.Items.Vanity.Excellence;
using BlockVanity.Content.Items.Vanity.Myrtle;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content;

public static class AllQuests
{
    public static void AddQuests(ref QuestDatabase database)
    {
        database.Clear();
        database.Add(new QuestEntry
        {
            NameIdentifier = "Myrtle",
            UnlockCondition = () => Main.LocalPlayer.HasItem(ModContent.ItemType<PlumeriaHairpin>()),
            ShowCondition = AlwaysAvailable,
            Icon = new ItemQuestIcon(ModContent.ItemType<PlumeriaHairpin>()),
            rewards = [
                new Item(ModContent.ItemType<MyrtleDress>()),
                new Item(ModContent.ItemType<MyrtleSandals>()),
                new Item(ModContent.ItemType<FishFood>())],

            reclaimable = true,
            reclaimPriceAdd = Item.buyPrice(0, 3, 50, 0)
        });

        database.Add(new QuestEntry
        {
            NameIdentifier = "Myrtle",
            UnlockCondition = () => Main.LocalPlayer.HasItem(ModContent.ItemType<Excellence>()),
            ShowCondition = AlwaysAvailable,
            Icon = new ItemQuestIcon(ModContent.ItemType<Excellence>()),
            rewards = [
                new Item(ModContent.ItemType<Excellence>())],

            reclaimable = true,
            reclaimPriceAdd = Item.buyPrice(0, 3, 50, 0)
        });
    }  

    private static bool AlwaysAvailable() => true;
}
