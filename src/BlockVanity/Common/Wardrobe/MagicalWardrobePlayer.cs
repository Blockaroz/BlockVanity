using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BlockVanity.Common.Wardrobe;

public sealed class MagicalWardrobePlayer : ModPlayer
{
    public int QuestsComplete { get; private set; }

    public int StarCount { get; private set; }

    public override void SaveData(TagCompound tag)
    {
    }

    public override void LoadData(TagCompound tag)
    {  
    }

    public void CalculateCompletion()
    {
        StarCount = 0;
        QuestsComplete = 0;
    }
}
