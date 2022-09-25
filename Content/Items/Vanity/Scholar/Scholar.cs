using BlockVanity.Content.Items.Vanity.BlockheadVanity;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Scholar
{
    [AutoloadEquip(EquipType.Head)]
    public class ScholarHood : VanityItem
    {
        public ScholarHood() : base("Scholar's Hood", ItemRarityID.Blue, "'Om...'") { }
    }

    public class ScholarHoodEyes : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
        
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.armor[10].IsAir && drawInfo.drawPlayer.armor[0].ModItem is ScholarHood || drawInfo.drawPlayer.armor[10].ModItem is ScholarHood;

        public override bool IsHeadLayer => true;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Asset<Texture2D> eyes = Mod.Assets.Request<Texture2D>("Content/Items/Vanity/Scholar/ScholarHood_Eyes");

            float colorStrength = MathHelper.Clamp(drawInfo.drawPlayer.eyeColor.R + drawInfo.drawPlayer.eyeColor.G + drawInfo.drawPlayer.eyeColor.B, 0, 255) / 255f;
            Color eyeColor = drawInfo.drawPlayer.eyeColor * Utils.GetLerpValue(0.4f, 0.6f, colorStrength, true);

            Rectangle eyeRect = eyes.Frame(1, 3, 0, drawInfo.drawPlayer.eyeHelper.EyeFrameToShow);
            Vector2 pos = drawInfo.HeadPosition();
            pos.ApplyVerticalOffset(drawInfo);

            DrawData eyeData = new DrawData(eyes.Value, pos, eyeRect, eyeColor, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            eyeData.shader = drawInfo.cHead;
            drawInfo.DrawDataCache.Add(eyeData);
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class ScholarCloak : VanityItem
    {
        public ScholarCloak() : base("Scholar's Cloak", ItemRarityID.Blue, "'Or...'") { }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class ScholarShorts : VanityItem
    {
        public ScholarShorts() : base("Scholar's Shorts", ItemRarityID.Blue, "'Oth...'") { }
    }
}
