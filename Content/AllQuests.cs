using System;
using BlockVanity.Common.Quests;
using BlockVanity.Content.Hairs;
using BlockVanity.Content.Items.Dyes;
using BlockVanity.Content.Items.Vanity.Excellence;
using BlockVanity.Content.Items.Vanity.Myrtle;
using BlockVanity.Content.Items.Vanity.Scholar;
using BlockVanity.Content.Items.Weapons.Magic;
using BlockVanity.Content.Quests;
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
        int totalStarCount = 0;
        database.Clear();

        database.Add(new CardboardBoxQuest());
        database.Add(new MyrtleQuest());
        database.Add(new ScholarQuest());

        for (int i = 0; i < database.Entries.Count; i++)
        {
            totalStarCount += database.Entries[i].StarCount;
            database.Entries[i].SetData(i);
        }

        database.TotalStarCount = totalStarCount;
    }
}
