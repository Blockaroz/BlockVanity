using Terraria.ModLoader;

namespace BlockVanity.Common.Graphics;

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