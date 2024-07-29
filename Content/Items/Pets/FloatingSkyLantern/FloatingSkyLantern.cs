using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.FloatingSkyLantern;

public class FloatingSkyLantern : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.DefaultToVanitypet(ModContent.ProjectileType<FloatingSkyLanternProjectile>(), ModContent.BuffType<FloatingSkyLanternBuff>());
    }
}
