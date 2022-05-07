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

namespace BlockVanity.Content.Items.Vanity.RedMushroomCow
{
    [AutoloadEquip(EquipType.Head)]
    public class RedMushroomCowHead : VanityItem
    {
        public RedMushroomCowHead() : base("Red Mushroom Cow Head", ItemRarityColor.LightRed4) { }

        public override bool IsVanitySet(int head, int body, int legs) => body == ModContent.ItemType<RedMushroomCowHide>() && legs == ModContent.ItemType<RedMushroomCowTrotters>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Mushroom, 15)
                .AddIngredient(ItemID.Stinger, 2)
                .AddIngredient(ItemID.RedandSilverDye)
                .AddIngredient(ItemID.Sunglasses)
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class RedMushroomCowHide : VanityItem
    {
        public RedMushroomCowHide() : base("Red Mushroom Cow Hide", ItemRarityColor.LightRed4) { }

        public override bool IsVanitySet(int head, int body, int legs) => head == ModContent.ItemType<RedMushroomCowHead>() && legs == ModContent.ItemType<RedMushroomCowTrotters>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Mushroom, 30)
                .AddIngredient(ItemID.Silk, 3)
                .AddIngredient(ItemID.RedandSilverDye)
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class RedMushroomCowTrotters : VanityItem
    {
        public RedMushroomCowTrotters() : base("Red Mushroom Cow Trotters", ItemRarityColor.LightRed4) { }

        public override bool IsVanitySet(int head, int body, int legs) => head == ModContent.ItemType<RedMushroomCowHead>() && body == ModContent.ItemType<RedMushroomCowHide>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Mushroom, 20)
                .AddIngredient(ItemID.Silk, 3)
                .AddIngredient(ItemID.RedandSilverDye)
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    //public class RedMushroomCowNeck : PlayerDrawLayer
    //{
    //    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

    //    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => (drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is RedMushroomCowHead) || drawInfo.drawPlayer.armor[10].ModItem is RedMushroomCowHead;

    //    public override bool IsHeadLayer => false;

    //    protected override void Draw(ref PlayerDrawSet drawInfo)
    //    {
    //        Asset<Texture2D> neck = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/RedMushroomCow/RedMushroomCowHead_Neck");
    //        Vector2 pos = drawInfo.HeadPosition();
    //        pos.ApplyVerticalOffset(drawInfo);
    //        drawInfo.DrawDataCache.Add(new DrawData(neck.Value, pos, null, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0));
    //    }
    //}
}
