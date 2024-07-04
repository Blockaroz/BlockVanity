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
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;

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
        Item.maxStack = 99;
        Item.value = Item.sellPrice(0, 1, 50);
        Item.rare = ItemRarityID.LightRed;
    }
}

public class OrionDye : BaseOrionDye
{
    public override Color MainColor => new Color(28, 155, 255);
    public override Color SecondaryColor => new Color(55, 39, 211);

    public override void AddRecipes()
    {
        CreateRecipe(3)
            .AddIngredient(ItemID.BottledWater)
            .AddIngredient(ItemID.LunarBar, 3)
            .AddTile(TileID.DyeVat)
            .Register();
    }
}

public class RedOrionDye : BaseOrionDye
{
    public override Color MainColor => new Color(255, 10, 10);
    public override Color SecondaryColor => new Color(240, 10, 30);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<OrionDye>()
            .AddIngredient(ItemID.RedDye)
            .AddTile(TileID.DyeVat)
            .Register();
    }
}

public class GreenOrionDye : BaseOrionDye
{
    public override Color MainColor => new Color(70, 255, 58);
    public override Color SecondaryColor => new Color(0, 165, 50);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<OrionDye>()
            .AddIngredient(ItemID.GreenDye)
            .AddTile(TileID.DyeVat)
            .Register();
    }
}

public class YellowOrionDye : BaseOrionDye
{
    public override Color MainColor => new Color(255, 200, 28);
    public override Color SecondaryColor => new Color(125, 97, 8);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<OrionDye>()
            .AddIngredient(ItemID.YellowDye)
            .AddTile(TileID.DyeVat)
            .Register();
    }
}

public class PurpleOrionDye : BaseOrionDye
{
    public override Color MainColor => new Color(255, 128, 255);
    public override Color SecondaryColor => new Color(105, 39, 234);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<OrionDye>()
            .AddIngredient(ItemID.PurpleDye)
            .AddTile(TileID.DyeVat)
            .Register();
    }
}

public class WhiteOrionDye : BaseOrionDye
{
    public override Color MainColor => Color.Gray;
    public override Color SecondaryColor => Color.LightSlateGray;

    public override void AddRecipes()
    {
        CreateRecipe(3)
            .AddIngredient<RedOrionDye>()
            .AddIngredient<GreenOrionDye>()
            .AddIngredient<OrionDye>()
            .AddTile(TileID.DyeVat)
            .Register();
    }
}
