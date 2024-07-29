using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Dyes;

public class RadiationDye : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
            GameShaders.Armor.BindShader(Type, new ArmorShaderData(Main.PixelShaderRef, "ArmorMartian")).UseColor(0.3f, 3f, 0.1f);
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
