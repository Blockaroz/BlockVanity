using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace BlockVanity.Common.Rarities;

public abstract class SpecialRarity : ModRarity
{
    public abstract void DrawRareLine(string text, Vector2 position, float rotation, Vector2 origin, Vector2 scale);
}