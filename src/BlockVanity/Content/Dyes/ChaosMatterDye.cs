using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dyes;

public class ChaosMatterDye : ModItem
{
    public static LazyAsset<Effect> ChaosMatterDyeEffect { get; } = new LazyAsset<Effect>($"{nameof(BlockVanity)}/Assets/Effects/Dyes/ChaosMatterDye");

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
        {
            GameShaders.Armor.BindShader(Type, new GlobalTimeDyeShaderData(ChaosMatterDyeEffect)
                .UseColor(new Color(101, 0, 255))
                .UseSecondaryColor(new Color(20, 0, 120))
                .UseImage(Assets.Textures.MiscNoise[0]));
        }
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