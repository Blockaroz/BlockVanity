using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities.GlowingRarities;

public abstract class SpecialRarity : ModRarity
{
    public abstract void DrawRareLine(string text, Vector2 position, float rotation, Vector2 origin, Vector2 scale);
}
