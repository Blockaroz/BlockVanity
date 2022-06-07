using BlockVanity.Core;
using Terraria.ModLoader;
using System;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using BlockVanity.Common.Players;

namespace BlockVanity.Content.Items.Vanity.BrownMushroomCow
{
    [AutoloadEquip(EquipType.Head)]
    public class BrownMushroomCowHead : VanityItem
    {
        public BrownMushroomCowHead() : base("Brown Mushroom Cow Head", ItemRarityColor.Green2) { }

        public override void PostStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BrownMushroomCowHide>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

        public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisuals>().brown = true;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<RedMushroomCow.RedMushroomCowHead>()
                .AddCondition(Recipe.Condition.InRain)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class BrownMushroomCowHide : VanityItem
    {
        public BrownMushroomCowHide() : base("Brown Mushroom Cow Hide", ItemRarityColor.Green2) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<BrownMushroomCowHead>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<RedMushroomCow.RedMushroomCowHide>()
                .AddCondition(Recipe.Condition.InRain)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class BrownMushroomCowTrotters : VanityItem
    {
        public BrownMushroomCowTrotters() : base("Brown Mushroom Cow Trotters", ItemRarityColor.Green2) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<BrownMushroomCowHead>() && body.type == ModContent.ItemType<BrownMushroomCowHide>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<RedMushroomCow.RedMushroomCowTrotters>()
                .AddCondition(Recipe.Condition.InRain)
                .Register();
        }
    }

    public class BrownMushroomCowNeck : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => (drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is BrownMushroomCowHead) || drawInfo.drawPlayer.armor[10].ModItem is BrownMushroomCowHead;

        public override bool IsHeadLayer => false;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Asset<Texture2D> neck = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/BrownMushroomCow/BrownMushroomCowHead_Neck");
            Vector2 pos = drawInfo.HeadPosition();
            pos.ApplyVerticalOffset(drawInfo);
            drawInfo.DrawDataCache.Add(new DrawData(neck.Value, pos, null, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0));
        }
    }
}
