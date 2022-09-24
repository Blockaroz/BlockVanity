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
        public PassionatePerfume() : base("Passionate Perfume", ItemRarityID.Blue, "Hearts follow your path \n'Made with love'", true) { }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 200);

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 1.5f)
            {
                int frequency = 14 - (int)(slot % 10f / 10f * 12f);
                if (Main.rand.NextBool(frequency))
                {
                    Color heartColor = new Color(255, 20, 10, 20);
                    if (dye != null)
                        heartColor = new Color(180, 180, 180, 120);
                    Particle heart = Particle.NewParticle(Particle.ParticleType<PopHeart>(), player.MountedCenter + Main.rand.NextVector2Circular(15, 20), player.velocity * 0.15f - Vector2.UnitY * Main.rand.NextFloat(), heartColor, 0.5f + Main.rand.NextFloat());
                    heart.emit = true;
                    heart.shader = dye;
                }
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
