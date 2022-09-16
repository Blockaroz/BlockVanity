using BlockVanity.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.FloatingSkyLantern
{
    public class FloatingSkyLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Floating Sky Lantern");
            Tooltip.SetDefault("Appears to attract fireflies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.DefaultToVanitypet(ModContent.ProjectileType<FloatingSkyLanternProj>(), ModContent.BuffType<FloatingSkyLanternBuff>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Torch, 5)
                .AddIngredient(ItemID.Silk, 12)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 255 - Item.alpha);
    }
}
