using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Systems.Players;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.Excellence;
using BlockVanity.Content.Items.Vanity.Myrtle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class FishSkinPlayer : ModPlayer
{
    public bool enabled;

    public static class FishSkinID
    {
        public static readonly int 
            Head = 0,
            Ears = 1,
            Eyes = 2,
            Body = 3,
            Legs = 4;
    }

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_12_Skin += DrawFishSkin;
        On_Player.UpdateVisibleAccessory += EnableFishSkin;
    }

    private void EnableFishSkin(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
    {
        orig(self, itemSlot, item, modded);

        if (item.ModItem is FishFood fishFood)
            self.GetModPlayer<FishSkinPlayer>().enabled = true;
    }

    private void DrawFishSkin(On_PlayerDrawLayers.orig_DrawPlayer_12_Skin orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
        {
            orig(ref drawinfo);

            Vector2 headPos = drawinfo.HeadPosition() - Vector2.UnitY * 2;
            headPos.ApplyVerticalOffset(drawinfo);

            float headOffY = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height].Y;
            bool useFallFrame = drawinfo.drawPlayer.GetModPlayer<ExcellencePlayer>().headAltFrame;

            Texture2D earsTexture = AllAssets.Textures.BlueFishSkin[FishSkinID.Ears].Value;
            DrawData headData = new DrawData(earsTexture, headPos, earsTexture.Frame(1, 2, 0, useFallFrame ? 1 : 0), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
            headData.shader = drawinfo.skinDyePacked;
            drawinfo.DrawDataCache.Add(headData);
        }
        else
            orig(ref drawinfo);
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}
