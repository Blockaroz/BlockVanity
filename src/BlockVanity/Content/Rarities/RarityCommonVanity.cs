using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities;

public class RarityCommonVanity : ModRarity
{
    public override Color RarityColor => Color.Lerp(new Color(105, 71, 255), new Color(136, 101, 255), MathF.Pow(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.PiOver2 % MathHelper.TwoPi), 2));
}