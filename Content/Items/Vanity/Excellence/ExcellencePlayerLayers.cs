﻿using BlockVanity.Common.Systems.Players;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.MushroomCow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Excellence;

public class ExcellencePlayerHeadLayer : PlayerDrawLayer
{
    public static Asset<Texture2D> headTexture;
    public static Asset<Texture2D> headGlowTexture;

    public override void Load()
    {
        string texturePath = $"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/Excellence/Excellence";
        headTexture = ModContent.Request<Texture2D>($"{texturePath}_{EquipType.Head}");
        headGlowTexture = ModContent.Request<Texture2D>($"{texturePath}_{EquipType.Head}_Glow");
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool IsHeadLayer => true;

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(Excellence), EquipType.Head);

    private float _oldHeadY;
    private int _oldFrame;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 headPos = drawInfo.HeadPosition() - Vector2.UnitY * 2 * drawInfo.drawPlayer.gravDir;
        headPos.ApplyVerticalOffset(drawInfo);

        float headOffY = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height].Y;
        bool useFallFrame = drawInfo.drawPlayer.velocity.Y * drawInfo.drawPlayer.gravDir > 0;

        DrawData headData = new DrawData(headTexture.Value, headPos, headTexture.Value.Frame(1, 2, 0, useFallFrame ? 1 : 0), drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        headData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(headData);

        if (drawInfo.shadow <= 0f)
        {
            DrawData headGlowData = new DrawData(headGlowTexture.Value, headPos, headTexture.Value.Frame(1, 2, 0, useFallFrame ? 1 : 0), Color.White, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            headGlowData.shader = drawInfo.cHead;
            drawInfo.DrawDataCache.Add(headGlowData);

            Color shakeColor = new Color(255, 255, 255, 0);
            for (int i = 0; i < 2; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(2, 2) * 0.67f;
                DrawData headGlowShakeData = new DrawData(headGlowTexture.Value, headPos + offset, headTexture.Value.Frame(1, 2, 0, useFallFrame ? 1 : 0), shakeColor, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
                headGlowShakeData.shader = drawInfo.cHead;
                drawInfo.DrawDataCache.Add(headGlowShakeData);
            }
        }
    }
}

public class ExcellencePlayerLegsLayer : PlayerDrawLayer
{
    public static Asset<Texture2D> legsTexture;

    public override void Load()
    {
        string texturePath = $"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/Excellence/Excellence";
        legsTexture = ModContent.Request<Texture2D>($"{texturePath}_{EquipType.Legs}");
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, nameof(Excellence), EquipType.Legs);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 legPos = drawInfo.LegsPosition() + drawInfo.legsOffset + new Vector2(0, drawInfo.drawPlayer.gravDir * 2);

        int legFrame = drawInfo.drawPlayer.legFrame.Y / drawInfo.drawPlayer.legFrame.Height - 3;

        int originOffY = 0;

        if (legFrame < 2)
            legFrame = 0;
        else if (legFrame == 2)
            legFrame = 1;

        if (drawInfo.isSitting)
        {
            legFrame = 2;
            originOffY = 4;
        }

        DrawData legData = new DrawData(legsTexture.Value, legPos, legsTexture.Value.Frame(1, 17, 0, legFrame), drawInfo.colorArmorLegs, drawInfo.drawPlayer.legRotation, new Vector2(drawInfo.legVect.X, drawInfo.legVect.Y + originOffY), 1f, drawInfo.playerEffect, 0);
        legData.shader = drawInfo.cLegs;
        drawInfo.DrawDataCache.Add(legData);
    }
}
