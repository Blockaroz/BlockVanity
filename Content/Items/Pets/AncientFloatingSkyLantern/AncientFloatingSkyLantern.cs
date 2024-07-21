using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.AncientFloatingSkyLantern;

public class AncientFloatingSkyLantern : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.DefaultToVanitypet(ModContent.ProjectileType<AncientFloatingSkyLanternProj>(), ModContent.BuffType<AncientFloatingSkyLanternBuff>());
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Torch, 5)
            .AddIngredient(ItemID.Silk, 20)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 255 - Item.alpha);
}
