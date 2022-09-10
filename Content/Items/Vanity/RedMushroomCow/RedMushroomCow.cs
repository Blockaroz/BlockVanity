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
using Terraria.Graphics.Shaders;

namespace BlockVanity.Content.Items.Vanity.RedMushroomCow
{
    [AutoloadEquip(EquipType.Head)]
    public class RedMushroomCowHead : VanityItem
    {
        public RedMushroomCowHead() : base("Red Mushroom Cow Head", ItemRarityID.Blue) { }

        public override void PostStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<RedMushroomCowHide>() && legs.type == ModContent.ItemType<RedMushroomCowTrotters>();

        public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisuals>().red = true;

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
        public RedMushroomCowHide() : base("Red Mushroom Cow Hide", ItemRarityID.Blue) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<RedMushroomCowHead>() && legs.type == ModContent.ItemType<RedMushroomCowTrotters>();

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
        public RedMushroomCowTrotters() : base("Red Mushroom Cow Trotters", ItemRarityID.Blue) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<RedMushroomCowHead>() && body.type == ModContent.ItemType<RedMushroomCowHide>();

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

    public class RedMushroomCowNeck : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => (drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is RedMushroomCowHead) || drawInfo.drawPlayer.armor[10].ModItem is RedMushroomCowHead;

        public override bool IsHeadLayer => false;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Asset<Texture2D> neck = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/RedMushroomCow/RedMushroomCowHead_Neck");
            Vector2 pos = drawInfo.HeadPosition();
            pos.ApplyVerticalOffset(drawInfo);
            DrawData neckData = new DrawData(neck.Value, pos, null, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            neckData.shader = drawInfo.cHead;
            drawInfo.DrawDataCache.Add(neckData);
        }
    }
}
