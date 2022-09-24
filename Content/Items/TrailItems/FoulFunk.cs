using BlockVanity.Content.Particles;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using ParticleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace BlockVanity.Content.Items.TrailItems
{
    public class FoulFunk : VanityItem, ITrailItem
    {
        public FoulFunk() : base("Foul Funk", ItemRarityID.Expert, "A horrible stench is left in your wake \n'Made without love'", true) { }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 200);

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 2f)
            {
                int frequency = 12 - (int)(slot % 10f / 10f * 4f);
                Color dungColor = Color.Lerp(Color.DarkOliveGreen, Color.Olive, Main.rand.NextFloat()) * 0.16f;
                dungColor.A += 50;
                Particle smog = Particle.NewParticle(Particle.ParticleType<CopyParticle>(), player.MountedCenter, player.velocity * 0.3f, dungColor, 1f + Main.rand.NextFloat());
                int goreType = Main.rand.Next(61, 64);
                Main.instance.LoadGore(goreType);
                smog.data = TextureAssets.Gore[goreType];

                if (Main.rand.NextBool(frequency))
                {
                    Color sporeColor = Color.Lerp(Color.PaleGreen, Color.Khaki, Main.rand.NextFloat());
                    Particle spore = Particle.NewParticle(Particle.ParticleType<DeepSpore>(), player.MountedCenter + Main.rand.NextVector2Circular(15, 20), player.velocity * 0.3f, sporeColor, Main.rand.NextFloat(0.5f));
                    spore.emit = true;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.Stinkfish, 5)
                .AddIngredient(ItemID.Blinkroot, 5)
                .AddIngredient(ItemID.Deathweed, 20)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
