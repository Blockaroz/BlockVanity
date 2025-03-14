using BlockVanity.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.MetalSonic;

public class SonicHeadLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool IsHeadLayer => true;

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Head);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D headTexture = TextureAssets.ArmorHead[EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Head)].Value;
        Rectangle headFrame = headTexture.Frame(1, 3, 0, (int)drawInfo.drawPlayer.eyeHelper.CurrentEyeFrame);

        Vector2 headPos = drawInfo.HeadPosition();
        headPos.ApplyVerticalOffset(drawInfo);
        Vector2 headVect = drawInfo.headVect + new Vector2(4, 0);

        DrawData headData = new DrawData(headTexture, headPos, headFrame, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, headVect, 1f, drawInfo.playerEffect, 0);
        headData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(headData);
    }
}

public class SonicLegsLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Legs);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D legsTexture = TextureAssets.ArmorLeg[EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Legs)].Value;
        Vector2 legPos = drawInfo.LegsPosition() + drawInfo.legsOffset + new Vector2(0, drawInfo.drawPlayer.gravDir * 2);

        int legFrame = drawInfo.drawPlayer.legFrame.Y / drawInfo.drawPlayer.legFrame.Height - 3;

        int originOffY = 0;

        if (legFrame < 2)
        {
            legFrame = 0;
        }
        else if (legFrame == 2)
        {
            legFrame = 1;
        }

        if (drawInfo.isSitting)
        {
            legFrame = 2;
            originOffY = 4;
        }

        DrawData legData = new DrawData(legsTexture, legPos, legsTexture.Frame(1, 17, 0, legFrame), drawInfo.colorArmorLegs, drawInfo.drawPlayer.legRotation, new Vector2(drawInfo.legVect.X, drawInfo.legVect.Y + originOffY), 1f, drawInfo.playerEffect, 0);
        legData.shader = drawInfo.cLegs;
        drawInfo.DrawDataCache.Add(legData);
    }
}