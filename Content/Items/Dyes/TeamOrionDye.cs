using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Dyes;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Dyes
{
    public class TeamOrionDye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Team Orion Dye");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

            if (!Main.dedServ)
            {
                Effect orionShader = ModContent.Request<Effect>($"{nameof(BlockVanity)}/Assets/Effects/OrionShader", AssetRequestMode.ImmediateLoad).Value;
                //orionShader.Parameters["uNoise"].SetValue(ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/SpaceNoise").Value);

                GameShaders.Armor.BindShader(ModContent.ItemType<TeamOrionDye>(), new TeamArmorShaderData(new Ref<Effect>(orionShader), "OrionPass"))
                    .UseColor(Color.GhostWhite)
                    .UseSecondaryColor(Color.DimGray)
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
                .AddIngredient(ItemID.TeamDye)
                .AddTile(TileID.DyeVat)
                .Register();
        }
    }
}
