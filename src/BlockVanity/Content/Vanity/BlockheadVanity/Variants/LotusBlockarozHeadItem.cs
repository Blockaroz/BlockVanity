using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Content.Vanity.BlockheadVanity;
using BlockVanity.Content.Vanity.Misc;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.BlockheadVanity.Variants;

[AutoloadEquip(EquipType.Head)]
public class LotusBlockarozHeadItem : VanityItem
{
    public override string Texture => Assets.Textures.Content.Vanity.BlockheadVanity.Variants.LotusBlockarozHead_Item.KEY;

    public override LocalizedText DisplayName => ModContent.GetInstance<BlockarozHeadItem>().DisplayName;

    public override int Rarity => ItemRarityID.Cyan;

    public override void SetStaticDefaults()
    {
        BlockheadHeadLayer.AddHead(this);
        NeckPlayerLayer.AddNeck(this);

        ArmorIDs.Head.Sets.HidesHead[Item.headSlot] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<BlockarozHeadItem>()
            .AddIngredient<LotusHairpinItem>()
            .Register();
    }
}