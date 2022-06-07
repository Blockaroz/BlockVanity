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

namespace BlockVanity.Content.Items.Vanity.GlowingMushroomCow
{
    [AutoloadEquip(EquipType.Head)]
    public class GlowingMushroomCowHead : VanityItem
    {
        public GlowingMushroomCowHead() : base("Glowing Mushroom Cow Head", ItemRarityColor.Green2) { }

        public override void PostStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<GlowingMushroomCowHide>() && legs.type == ModContent.ItemType<GlowingMushroomCowTrotters>();

        public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisuals>().glowing = true;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 50)
                .AddIngredient<RedMushroomCow.RedMushroomCowHead>()
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class GlowingMushroomCowHide : VanityItem
    {
        public GlowingMushroomCowHide() : base("Glowing Mushroom Cow Hide", ItemRarityColor.Green2) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<GlowingMushroomCowHead>() && legs.type == ModContent.ItemType<GlowingMushroomCowTrotters>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 90)
                .AddIngredient<RedMushroomCow.RedMushroomCowHide>()
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class GlowingMushroomCowTrotters : VanityItem
    {
        public GlowingMushroomCowTrotters() : base("Glowing Mushroom Cow Trotters", ItemRarityColor.Green2) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<GlowingMushroomCowHead>() && body.type == ModContent.ItemType<GlowingMushroomCowHide>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 70)
                .AddIngredient<RedMushroomCow.RedMushroomCowTrotters>()
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    public class GlowingMushroomCowNeck : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => (drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is GlowingMushroomCowHead) || drawInfo.drawPlayer.armor[10].ModItem is GlowingMushroomCowHead;

        public override bool IsHeadLayer => false;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Asset<Texture2D> neck = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/GlowingMushroomCow/GlowingMushroomCowHead_Neck");
            Vector2 pos = drawInfo.HeadPosition();
            pos.ApplyVerticalOffset(drawInfo);
            drawInfo.DrawDataCache.Add(new DrawData(neck.Value, pos, null, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0));
        }
    }
}
