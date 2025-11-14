using BlockVanity.Content.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BlockVanity.Common.Wardrobe;

public sealed class MagicalWardrobePlayer : ModPlayer
{
    public int QuestsAvailable { get; private set; }
    public int QuestsComplete { get; private set; }

    public int StarsAvailable { get; private set; }
    public int StarCount { get; private set; }

    public override void SaveData(TagCompound tag)
    {
    }

    public override void LoadData(TagCompound tag)
    {  
    }

    public void UpdateStatus()
    {
        QuestsAvailable = 0;
        QuestsComplete = 0;
        StarsAvailable = 0;
        StarCount = 0;

        for (int i = 0; i < MagicalWardrobe.Entries.Count; i++)
        {
            var entry = MagicalWardrobe.Entries[i];


            if (!entry.IsLocked)
            {
                QuestsAvailable++;
                StarsAvailable += entry.Stars;
                if (entry.IsCompleted)
                {
                    QuestsComplete++;
                    StarCount += entry.Stars;
                }
            }
        }
    }
}
