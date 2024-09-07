using System;
using BlockVanity.Common.Quests;
using BlockVanity.Content.Hairs;
using BlockVanity.Content.Items.Dyes;
using BlockVanity.Content.Items.TestItems;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.Myrtle;
using BlockVanity.Content.Items.Vanity.Scholar;
using BlockVanity.Content.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace BlockVanity.Content;

public static class AllQuests
{
    public static readonly int StarCountCommon = 1;
    public static readonly int StarCountUncommon = 4;
    public static readonly int StarCountRare = 5;

    public static void AddQuests(ref QuestDatabase database)
    {
        database.Clear();

        database.Add(new QuestEntry
        {
            NameKey = "CardboardBox",
            CompleteCondition = () => true,
            //AvailableCondition = null,
            StarCount = StarCountCommon,
            Icon = new ItemQuestIcon(ItemType<CardboardBox>()),
            Portrait = new PlayerQuestIcon(CreatePlayer(head: ItemType<CardboardBox>())),
            Reward = new QuestRewardData([
                ItemType<CardboardBox>()], Item.buyPrice(0, 3, 50, 0))
        });

        Player myrtlePlayer = CreatePlayer(head: ItemType<PlumeriaHairpin>(), body: ItemType<MyrtleDress>(), legs: ItemType<MyrtleSandals>(), acc1: ItemType<FishFood>());
        myrtlePlayer.hair = GetInstance<FishyHair>().Type;
        myrtlePlayer.hairDye = ContentSamples.ItemsByType[ItemType<SeasideHairDye>()].hairDye;
        database.Add(new QuestEntry
        {
            NameKey = "Myrtle",
            CompleteCondition = () => Main.LocalPlayer.HasItem(ItemType<PlumeriaHairpin>()),
            StarCount = StarCountUncommon,
            Icon = new PlayerQuestIcon(myrtlePlayer),
            Reward = new QuestRewardData([
                ItemType<MyrtleDress>(),
                ItemType<MyrtleSandals>(),
                ItemType<FishFood>(),
                ItemType<SeasideHairDye>()], Item.buyPrice(0, 3, 50, 0))
        });

        Player scholarPlayer = CreatePlayer(head: ItemType<ScholarHood>(), body: ItemType<ScholarCloak>());
        scholarPlayer.eyeColor = Color.Red;
        database.Add(new QuestEntry
        {
            NameKey = "CaveScholar",
            AvailableCondition = () => Main.hardMode,
            CompleteCondition = () => true,
            StarCount = StarCountRare,
            Icon = new PlayerQuestIcon(scholarPlayer),
            Reward = new QuestRewardData([
                ItemType<ScholarStaff>()], Item.buyPrice(0, 5, 0, 0))
        });

        Player lichPlayer = CreatePlayer(head: ItemID.Skull);
        database.Add(new QuestEntry
        {
            NameKey = "RoyalLich",
            AvailableCondition = () => Main.hardMode,
            CompleteCondition = () => true,
            SkipFreeClaim = true,
            StarCount = StarCountRare,
            Icon = new PlayerQuestIcon(lichPlayer),
            Reward = new QuestRewardData([
                ItemType<ScholarStaff>()], Item.buyPrice(0, 5, 0, 0))
        });

        for (int i = 0; i < 100; i++)
        {
            database.Add(new QuestEntry
            {
                NameKey = "TestQuest",
                CompleteCondition = Main.rand.NextBool() ? () => true : () => false,
                AvailableCondition = Main.rand.NextBool() ? () => true : () => false,
                StarCount = i / 6,
                Icon = new ItemQuestIcon(ItemType<QuestCompleter>())
            });
        }

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
