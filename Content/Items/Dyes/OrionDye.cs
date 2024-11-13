using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Dyes;

public abstract class BaseOrionDye : ModItem
{
    public virtual Color MainColor => Color.White;
    public virtual Color SecondaryColor => Color.DimGray;

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
        {
            GameShaders.Armor.BindShader(Type, new OrionShaderData())
                .UseColor(MainColor)
                .UseSecondaryColor(SecondaryColor);
        }
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(0, 1, 50);
        Item.rare = ItemRarityID.LightRed;
    }
}

//public class OrionDye : BaseOrionDye
//{
//    public override Color MainColor => new Color(28, 155, 255);
//    public override Color SecondaryColor => new Color(55, 39, 211);

//    public override void AddRecipes()
//    {
//        CreateRecipe(3)
//            .AddIngredient(ItemID.BottledWater)
//            .AddIngredient(ItemID.LunarBar, 3)
//            .AddTile(TileID.DyeVat)
//            .Register();
//    }
//}
