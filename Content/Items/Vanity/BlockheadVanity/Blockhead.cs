using BlockVanity.Core;
using Terraria.ModLoader;
using System;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.Audio;
using BlockVanity.Content.Items.Vanity.GlowingMushroomCow;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.GameContent;
using BlockVanity.Common.Players;

namespace BlockVanity.Content.Items.Vanity.BlockheadVanity
{
    [AutoloadEquip(EquipType.Head)]
    public class Blockhead : VanityItem
    {
        public Blockhead() : base("Block Head", ItemRarityID.Green) { }

        public override void PostStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        public override Color? GetAlpha(Color lightColor) => Main.LocalPlayer.skinColor.MultiplyRGBA(lightColor);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Cardboard>(16)
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ItemID.Lens, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class BlockheadNeck : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is Blockhead || drawInfo.drawPlayer.armor[10].ModItem is Blockhead;

        public override bool IsHeadLayer => false;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Asset<Texture2D> neck = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/BlockheadVanity/Blockhead_Neck");
            Vector2 pos = drawInfo.HeadPosition();
            pos.ApplyVerticalOffset(drawInfo);

            DrawData neckData = new DrawData(neck.Value, pos, null, drawInfo.colorBodySkin, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            neckData.shader = drawInfo.cHead;
            drawInfo.DrawDataCache.Add(neckData);
        }
    }

    public class BlockheadHead : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is Blockhead || drawInfo.drawPlayer.armor[10].ModItem is Blockhead;

        public override bool IsHeadLayer => true;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Asset<Texture2D> head = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/BlockheadVanity/Blockhead_Head");
            Asset<Texture2D> sclera = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/BlockheadVanity/Blockhead_Eyes0");
            Asset<Texture2D> iris = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/BlockheadVanity/Blockhead_Eyes1");
            Asset<Texture2D> eyelid = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/BlockheadVanity/Blockhead_Eyes2");

            Vector2 pos = drawInfo.HeadPosition();
            //pos.ApplyVerticalOffset(drawInfo);
            Rectangle headFrame = drawInfo.drawPlayer.legFrame;
            Rectangle eyeRect = eyelid.Frame(1, 3, 0, drawInfo.drawPlayer.eyeHelper.EyeFrameToShow);

            Color scleraColor = drawInfo.drawPlayer.GetModPlayer<MiscEffectPlayer>().accBlackEye ? Color.Black : drawInfo.colorEyeWhites;
            DrawData headData = new DrawData(head.Value, pos, headFrame, drawInfo.colorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            DrawData scleraData = new DrawData(sclera.Value, pos, headFrame, scleraColor, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            DrawData irisData = new DrawData(iris.Value, pos, headFrame, drawInfo.colorEyes, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            DrawData eyelidData = new DrawData(eyelid.Value, pos, eyeRect, drawInfo.colorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);

            headData.shader = drawInfo.cHead;
            scleraData.shader = drawInfo.cHead;
            irisData.shader = drawInfo.cHead;
            eyelidData.shader = drawInfo.cHead;

            drawInfo.DrawDataCache.Add(headData);
            drawInfo.DrawDataCache.Add(scleraData);
            drawInfo.DrawDataCache.Add(irisData);
            drawInfo.DrawDataCache.Add(eyelidData);
        }
    }
}
