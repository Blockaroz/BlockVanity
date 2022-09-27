using BlockVanity.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace BlockVanity.Content.Items.TrailItems.Omnibag
{
    public class AncientBagBlood : VanityItem, ITrailItem
    {
        public AncientBagBlood() : base("Ancient Bag of Blood", ItemRarityID.Green, "Blood and Ichor drip behind you", true) { }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 12)
                .AddIngredient(ItemID.Ichor, 10)
                .AddIngredient(ItemID.Vertebrae, 15)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 2f)
            {
                Lighting.AddLight(player.MountedCenter, Color.Gold.ToVector3() * 0.3f);

                int freq = (int)(slot % 10f / 10f * 4f);

                for (int i = 0; i < freq; i++)
                {
                    Dust d = Dust.NewDustPerfect(player.MountedCenter + Main.rand.NextVector2Circular(15, 20), DustID.Blood, Main.rand.NextVector2Circular(2, 2) + player.velocity * -0.4f, 0, Color.White, 1f + Main.rand.NextFloat());
                    d.noGravity = true;
                    d.shader = dye;
                }

                if (Main.rand.NextBool(6 - freq))
                {
                    int t = Main.rand.NextBool() ? DustID.Ichor : DustID.CrimsonTorch;
                    Dust d = Dust.NewDustPerfect(player.MountedCenter + Main.rand.NextVector2Circular(15, 20), t, Main.rand.NextVector2Circular(2, 2) + player.velocity * -0.4f, 0, Color.White, 2f);
                    d.noGravity = true;
                    d.shader = dye;
                }
            }
        }
    }
}
