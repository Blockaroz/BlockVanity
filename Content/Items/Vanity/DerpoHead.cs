using BlockVanity.Core;
using Terraria.ModLoader;
using System;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.Audio;
using BlockVanity.Common.Players;

namespace BlockVanity.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class DerpoHead : VanityItem
    {
        public DerpoHead() : base("Cool and Derpy Fish Head", ItemRarityID.Blue, "Hello all you magnificent people!\n'Great for impersonating content creators!'") { }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Goldfish, 20)
                .AddIngredient(ItemID.Sunglasses)
                .Register();
        }
    }
}
