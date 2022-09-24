using BlockVanity.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.ImpishEye
{
    public class LooseEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loose Eye");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.DefaultToVanitypet(ModContent.ProjectileType<ImpishEyeProj>(), ModContent.BuffType<ImpishEyeBuff>());
        }
    }
}
