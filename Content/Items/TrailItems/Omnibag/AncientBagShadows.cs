using BlockVanity.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace BlockVanity.Content.Items.TrailItems.Omnibag
{
    public class AncientBagShadows : VanityItem, ITrailItem
    {
        public AncientBagShadows() : base("Ancient Bag of Shadows", ItemRarityID.Green, "Shadows and Vileness follows behind you", true) { }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 12)
                .AddIngredient(ItemID.CursedFlame, 10)
                .AddIngredient(ItemID.RottenChunk, 15)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 2f)
            {
                Lighting.AddLight(player.MountedCenter, Color.DarkSeaGreen.ToVector3() * 0.3f);

                int freq = (int)(slot % 10f / 10f * 4f);

                for (int i = 0; i < freq; i++)
                {
                    Dust d = Dust.NewDustPerfect(player.MountedCenter + Main.rand.NextVector2Circular(15, 20), DustID.Corruption, Main.rand.NextVector2Circular(2, 2) + player.velocity * -0.5f, 0, Color.White, 1f);
                    d.noGravity = true;
                    d.shader = dye;
                }                
                
                if (Main.rand.NextBool(6 - freq))
                {
                    int t = Main.rand.NextBool() ? DustID.CursedTorch : DustID.ShadowbeamStaff;
                    Dust d = Dust.NewDustPerfect(player.MountedCenter + Main.rand.NextVector2Circular(15, 20), t, Main.rand.NextVector2Circular(2, 2) + player.velocity * -0.5f, 0, Color.White, 2f);
                    d.noGravity = true;
                    d.shader = dye;
                }                
            }
        }
    }
}
