using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dyes;

public class PhantomDye : ModItem
{
    public static LazyAsset<Effect> PhantomDyeEffect { get; } = new LazyAsset<Effect>($"{nameof(BlockVanity)}/Assets/Effects/Dyes/PhantomDye");

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
        {
            GameShaders.Armor.BindShader(Type, new GlobalTimeDyeShaderData(PhantomDyeEffect)
                .UseColor(new Color(79, 255, 211))
                .UseSecondaryColor(new Color(49, 115, 150))
                .UseImage(Main.Assets.Request<Texture2D>("Images/Misc/noise")));
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