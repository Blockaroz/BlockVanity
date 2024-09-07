using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Dyes;

public class PhantomDye : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
            GameShaders.Armor.BindShader(Type, new PhantomDyeShaderData()
                .UseColor(new Color(79, 255, 211)).UseSecondaryColor(new Color(49, 115, 150)));
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 99;
        Item.value = Item.sellPrice(0, 1, 50);
        Item.rare = ItemRarityID.Orange;
    }
}
