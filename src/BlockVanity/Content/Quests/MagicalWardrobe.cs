using BlockVanity.Common.Wardrobe;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.Excellence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Quests;

public static class MagicalWardrobe
{
    public static List<QuestEntry> Entries { get; } = [];

    public static void Create()
    {    
        static bool AlwaysAvailable(Player player) => true;

        Entries.Add(new QuestEntry(BlockVanity.Instance, "The Box", Stars: 1,
            new WardrobeItemPortrait(ModContent.ItemType<CardboardBox>()),
            AlwaysAvailable,
            player => player.HasItem(ModContent.ItemType<CardboardBox>())));

        Entries.Add(new QuestEntry(BlockVanity.Instance, "Excellence", Stars: 10,
            new WardrobeItemPortrait(ModContent.ItemType<Excellence>()),
            AlwaysAvailable,
            player => player.HasItem(ModContent.ItemType<CardboardBox>())));
    }

    public static void UpdateStatus()
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            var entry = Entries[i];

            entry.CheckAvailability(Main.LocalPlayer);
        }
    }
}
