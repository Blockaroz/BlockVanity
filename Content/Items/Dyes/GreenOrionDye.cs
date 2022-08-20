using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Dyes
{
    public class GreenOrionDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Green Orion Dye");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

            if (!Main.dedServ)
            {
                Effect orionShader = ModContent.Request<Effect>($"{nameof(BlockVanity)}/Assets/Effects/OrionShader", AssetRequestMode.ImmediateLoad).Value;
                //orionShader.Parameters["uNoise"].SetValue(ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/SpaceNoise").Value);

                GameShaders.Armor.BindShader(ModContent.ItemType<GreenOrionDye>(), new ArmorShaderData(new Ref<Effect>(orionShader), "OrionPass"))
                    .UseColor(new Color(70, 255, 58))
                    .UseSecondaryColor(new Color(0, 182, 167))
                    .UseImage("Images/Misc/noise")
                    .Shader.Parameters["uNoise"].SetValue(ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/SpaceNoise").Value);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(0, 1, 50);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<OrionDye>()
                .AddIngredient(ItemID.GreenDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}
