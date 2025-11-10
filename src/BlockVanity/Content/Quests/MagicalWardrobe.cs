using BlockVanity.Common.Utilities;
using BlockVanity.Common.Wardrobe;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.Excellence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;

namespace BlockVanity.Content.Quests;

public static class MagicalWardrobe
{
    public static List<QuestEntry> Entries { get; } = [];

    public static int TotalStarCount { get; private set; }

    public static void Populate()
    {    
        Entries.Add(QuestDefinitions.TheBox());
        Entries.Add(QuestDefinitions.Excellence());

        UpdateStatus();
    }

    public static void UpdateStatus()
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            var entry = Entries[i];
        }
    }
}
