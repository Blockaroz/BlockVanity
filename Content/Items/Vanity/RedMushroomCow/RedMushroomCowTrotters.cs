using BlockVanity.Core;
using Terraria.ModLoader;
using System;
using Terraria.Enums;
using Terraria.ID;
using Terraria;
using Terraria.Audio;

namespace BlockVanity.Content.Items.Vanity.RedMushroomCow
{
    [AutoloadEquip(EquipType.Legs)]
    public class RedMushroomCowTrotters : VanityItem
    {
        public RedMushroomCowTrotters() : base("Red Mushroom Cow Trotters", ItemRarityColor.LightRed4) { }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Leather, 8)
                .AddIngredient(ItemID.Silk, 3)
                .AddIngredient(ItemID.RedandSilverDye)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}
