using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities;

public class CommonVanityRarity : ModRarity
{
    public override Color RarityColor => Color.Lerp(
        new Color(105, 71, 255), 
        new Color(136, 101, 255), 
        MathF.Pow(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.PiOver2 % MathHelper.TwoPi), 2));
}

public class ToughVanityRarity : ModRarity
{
    public override Color RarityColor => Color.Lerp(
        new Color(205, 171, 55), 
        new Color(236, 151, 55), 
        MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.PiOver2 % MathHelper.TwoPi) * 0.5f + 0.5f);
}