using BlockVanity.Content.Rarities.GlowingRarities;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities.SpecialRarities;

public class SpecialRarityItem : GlobalItem
{
    public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
    {
        if (line.Mod == "Terraria" && line.Name == "ItemName")
        {
            if (RarityLoader.GetRarity(item.rare) is SpecialRarity specialRarity)
            {
                specialRarity.DrawRareLine(line.Text, new Vector2(line.X, line.Y + yOffset), line.Rotation, line.Origin, line.BaseScale);
                return false;
            }
        }

        return base.PreDrawTooltipLine(item, line, ref yOffset);
    }
}