using BlockVanity.Core;
using Terraria.ModLoader;
using System;
using Terraria.Enums;
using Terraria.ID;
using Terraria;
using Terraria.Audio;

namespace BlockVanity.Content.Items.Vanity.RedMushroomCow
{
    [AutoloadEquip(EquipType.Body)]
    public class RedMushroomCowHide : VanityItem
    {
        public RedMushroomCowHide() : base("Red Mushroom Cow Hide", ItemRarityColor.LightRed4) { }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Mushroom, 30)
                .AddIngredient(ItemID.Silk, 3)
                .AddIngredient(ItemID.RedandSilverDye)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}
