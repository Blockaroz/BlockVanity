using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Dyes;

public class ChaosMatterDye : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
        {
            GameShaders.Armor.BindShader(Type, new TimeDyeShaderData(AllAssets.Effects.ChaosMatterDye)
                .UseColor(new Color(101, 0, 255)).UseSecondaryColor(new Color(20, 0, 120)).UseImage(AllAssets.Textures.MiscNoise[0]));
        }
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(0, 1, 50);
        Item.rare = ItemRarityID.Orange;
    }
}
