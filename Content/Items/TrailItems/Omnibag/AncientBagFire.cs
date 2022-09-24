using BlockVanity.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace BlockVanity.Content.Items.TrailItems.Omnibag
{
    public class AncientBagFire : VanityItem, ITrailItem
    {
        public AncientBagFire(): base("Ancient Bag of Fire", ItemRarityID.Green, "Flames follow your path", true) { }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddIngredient(ItemID.Fireblossom, 15)
                .AddIngredient(ItemID.AshBlock, 50)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 2f)
            {
                int freq = (int)(slot % 10f / 10f * 4f);

                for (int i = 0; i < freq; i++)
                {
                    Dust d = Dust.NewDustPerfect(player.MountedCenter + Main.rand.NextVector2Circular(15, 20), DustID.Torch, Main.rand.NextVector2Circular(2, 2) + player.velocity * 0.1f, 0, Color.White, 2f);
                    d.noGravity = true;
                    d.shader = dye;
                }
            }
        }
    }
}
