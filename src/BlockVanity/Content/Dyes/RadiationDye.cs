using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dyes;

public class RadiationDye : ModItem
{
    public static LazyAsset<Effect> RadiationDyeEffect { get; } = new LazyAsset<Effect>($"{nameof(BlockVanity)}/Assets/Effects/Dyes/RadiationDye");

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
            GameShaders.Armor.BindShader(Type, new ArmorShaderData(RadiationDyeEffect, "ShaderPass")
                .UseColor(1.6f, 3f, 0.3f)
                .UseSecondaryColor(1.3f, 3f, 1.3f));
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(gold: 1, silver: 50);
        Item.rare = ItemRarityID.Orange;
    }
}