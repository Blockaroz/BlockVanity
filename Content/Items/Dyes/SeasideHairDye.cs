﻿using BlockVanity.Common.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Dyes;

public class SeasideHairDye : ModItem
{
    public override void SetStaticDefaults()
    {
        GameShaders.Hair.BindShader(Type, new SeasideHairShaderData());
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
