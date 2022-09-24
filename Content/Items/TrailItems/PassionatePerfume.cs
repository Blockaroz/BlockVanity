using BlockVanity.Content.Particles;
using BlockVanity.Core;
using Terraria;
using Microsoft.Xna.Framework;
using ParticleEngine;
using Terraria.ID;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Content.Items.TrailItems
{
    public class PassionatePerfume : VanityItem, ITrailItem
    {
        public PassionatePerfume() : base("Passionate Perfume", ItemRarityID.Expert, "Hearts follow your path \n'Made with love'", true) { }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 200);

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 1.5f)
            {
                int frequency = 10 - (int)(slot % 10f / 10f * 5f);
                if (Main.rand.NextBool(frequency))
                    Gore.NewGorePerfect(player.GetSource_Accessory(Item), player.MountedCenter + Main.rand.NextVector2Circular(18, 24), -Vector2.UnitY.RotatedByRandom(0.3f) + player.velocity * 0.5f, 331, Main.rand.NextFloat(0.5f, 1f));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.PrincessFish, 5)
                .AddIngredient(ItemID.Blinkroot, 5)
                .AddIngredient(ItemID.Shiverthorn, 20)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
