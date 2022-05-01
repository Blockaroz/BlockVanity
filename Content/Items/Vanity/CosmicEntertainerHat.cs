using BlockVanity.Core;
using Terraria.ModLoader;
using System;
using Terraria.Enums;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlockVanity.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class CosmicEntertainerHat : VanityItem
    {
        public CosmicEntertainerHat() : base("Cosmic Entertainer's Hat", ItemRarityColor.StrongRed10, "Where my hat offset methods at") { }
    }

    //public class CosmicEntertainerHatLayer : PlayerDrawLayer
    //{
    //    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    //    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => (drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is CosmicEntertainerHat) || drawInfo.drawPlayer.armor[10].ModItem is CosmicEntertainerHat;

    //    public override bool IsHeadLayer => true;

    //    protected override void Draw(ref PlayerDrawSet drawInfo)
    //    {
    //        DrawHat(ref drawInfo);
    //        DrawHatGlow(ref drawInfo);
    //    }

    //    private void DrawHat(ref PlayerDrawSet drawInfo)
    //    {
    //        Asset<Texture2D> hat = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/CosmicEntertainerHat_Head");
    //        Vector2 drawPos = drawInfo.HeadPosition() + Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
    //        drawPos += new Vector2(6, -4);
    //        DrawData hatData = new DrawData(hat.Value, drawPos, null, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
    //        drawInfo.DrawDataCache.Add(hatData);
    //    }

    //    private void DrawHatGlow(ref PlayerDrawSet drawInfo)
    //    {
    //        Asset<Texture2D> hat = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/CosmicEntertainerHat_Glow");
    //        Vector2 drawPos = drawInfo.HeadPosition() + Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
    //        drawPos += new Vector2(6, -4);

    //        for (int i = 0; i < 4; i++)
    //        {
    //            Color rainbow = Main.hslToRgb(((drawInfo.drawPlayer.miscCounter * 0.01f) + (i * 2)) % 1f, 0.66f, 0.66f, 0);
    //            Vector2 offset = new Vector2(1f, 0f).RotatedBy(MathHelper.TwoPi / 4f * i);
    //            DrawData hatData = new DrawData(hat.Value, drawPos + offset, null, rainbow, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
    //            //drawInfo.DrawDataCache.Add(hatData);
    //        }
    //    }
    //}
}
