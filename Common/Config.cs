using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace BlockVanity.Common;

public class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header($"Mods.{nameof(BlockVanity)}.Config.WardrobeUIPosition")]
    public Vector2 wardrobeUIPosition;
}
