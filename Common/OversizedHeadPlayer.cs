using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common;

public class OversizedHeadPlayer : ModPlayer
{
    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (EquipLoader.GetEquipTexture(EquipType.Head, Player.head) is OversizedHeadEquipTexture equipTexture)
        {
            drawInfo.helmetOffset += new Vector2(equipTexture.OffsetX * Player.direction, equipTexture.OffsetY * Player.gravDir);
        }
    }
}
