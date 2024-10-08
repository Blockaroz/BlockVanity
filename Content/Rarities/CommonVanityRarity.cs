using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities;

public class CommonVanityRarity : ModRarity
{
    public override Color RarityColor => Color.Lerp(new Color(105, 71, 255), new Color(136, 101, 255), MathF.Pow(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.PiOver2 % MathHelper.TwoPi), 2));
}
