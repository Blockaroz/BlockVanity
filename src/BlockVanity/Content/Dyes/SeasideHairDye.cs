using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dyes;

public sealed class SeasideHairDye : ModItem
{
    public static LazyAsset<Effect> SeasideHairDyeEffect { get; } = new LazyAsset<Effect>($"{nameof(BlockVanity)}/Assets/Effects/Dyes/SeasideHairDye");

    public override void SetStaticDefaults()
    {
        GameShaders.Hair.BindShader(Type, new SeasideHairShaderData().UseImage(Assets.Textures.SeasideColorMap));
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 26;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.buyPrice(gold: 5);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item3;
        Item.useStyle = ItemUseStyleID.DrinkLiquid;
        Item.useTurn = true;
        Item.useAnimation = 17;
        Item.useTime = 17;
        Item.consumable = true;
    }
}

public sealed class SeasideHairShaderData : HairShaderData
{
    public SeasideHairShaderData() : base(SeasideHairDye.SeasideHairDyeEffect, "ShaderPass") { }

    public override Color GetColor(Player player, Color lightColor) => lightColor;

    public override void Apply(Player player, DrawData? drawData = null)
    {
        base.Apply(player, drawData);
        Shader.Parameters["uTime"]?.SetValue(Main.GlobalTimeWrappedHourly / 2f);
        Shader.CurrentTechnique.Passes[0].Apply();
    }
}