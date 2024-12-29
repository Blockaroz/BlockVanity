using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BlockVanity.Core;

public class OversizedHeadEquipTexture : EquipTexture
{
    public OversizedHeadEquipTexture(int offsetX, int offsetY)
    {
        OffsetX = offsetX;
        OffsetY = offsetY;
    }

    public int OffsetX { get; }
    public int OffsetY { get; }
}
