using System;
using BlockVanity.Common.Quests;
using BlockVanity.Content.Hairs;
using BlockVanity.Content.Items.Dyes;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.Myrtle;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace BlockVanity.Content;

public static class AllQuests
{
    public static void AddQuests(ref QuestDatabase database)
    {
        database.Clear();

        database.Add(new QuestEntry
        {
            NameKey = "CardboardBox",
            CompleteCondition = () => true,
            //AvailableCondition = null,
            Icon = new ItemQuestIcon(ItemType<CardboardBox>()),
            Portrait = new PlayerQuestIcon(CreatePlayer(head: ItemType<CardboardBox>())),
            Rewards = [
                ItemType<CardboardBox>()],

            Reclaimable = true,
            ReclaimPrice = Item.buyPrice(0, 3, 50, 0),
            StarCount = 1
        });

        Player myrtlePlayer = CreatePlayer(ItemType<PlumeriaHairpin>(), ItemType<MyrtleDress>(), ItemType<MyrtleSandals>(), ItemType<FishFood>());
        myrtlePlayer.hair = GetInstance<FishyHair>().Type;
        myrtlePlayer.hairDye = ContentSamples.ItemsByType[ItemType<SeasideHairDye>()].hairDye;
        database.Add(new QuestEntry
        {
            NameKey = "Myrtle",
            CompleteCondition = () => Main.LocalPlayer.HasItem(ItemType<PlumeriaHairpin>()),
            Icon = new PlayerQuestIcon(myrtlePlayer),
            Rewards = [
                ItemType<MyrtleDress>(),
                ItemType<MyrtleSandals>(),
                ItemType<FishFood>(),
                ItemType<SeasideHairDye>()],

            Reclaimable = true,
            ReclaimPrice = Item.buyPrice(0, 3, 50, 0)
        });

        SetDatabaseIDs(ref database);
    }

    private static void SetDatabaseIDs(ref QuestDatabase database)
    {
        int totalStarCount = 0;

        for (int i = 0; i < database.Entries.Count; i++)
        {
            totalStarCount += database.Entries[i].StarCount;
            database.Entries[i].id = i;
            database.Entries[i].StarCount = Math.Max(database.Entries[i].StarCount, 1); // Minimum star count
            _ = database.Entries[i].Name.Value;
            _ = database.Entries[i].Description.Value;
        }

        database.TotalStarCount = totalStarCount;
    }

    private static Player CreatePlayer(int head = -1, int body = -1, int legs = -1, int acc1 = -1, int acc2 = -1)
    {
        Player newPlayer = new Player();
        if (head > -1)
            newPlayer.armor[10] = ContentSamples.ItemsByType[head];
        if (body > -1)
            newPlayer.armor[11] = ContentSamples.ItemsByType[body];
        if (legs > -1)
            newPlayer.armor[12] = ContentSamples.ItemsByType[legs];
        if (acc1 > -1)
            newPlayer.armor[13] = ContentSamples.ItemsByType[acc1];
        if (acc2 > -1)
            newPlayer.armor[14] = ContentSamples.ItemsByType[acc2];

        return newPlayer;
    }
}
