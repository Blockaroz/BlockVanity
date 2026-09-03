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
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
        {
            GameShaders.Armor.BindShader(Type, new GlobalTimeDyeShaderData(Assets.Effects.Dyes.PhantomDye.Asset)
                .UseColor(new Color(79, 255, 211))
                .UseSecondaryColor(new Color(49, 115, 150))
                .UseImage(Assets.Textures.Extras.Noise_Genuine.Asset));
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