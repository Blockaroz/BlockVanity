using Terraria.ModLoader.Config;

namespace BlockVanity.Common;

public class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    //[Header($"Mods.{nameof(BlockVanity)}.Config.WardrobeUIPosition")]
    //public Vector2 wardrobeUIPosition;
}