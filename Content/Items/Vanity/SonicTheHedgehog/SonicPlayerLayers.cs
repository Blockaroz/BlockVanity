using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Players;
using BlockVanity.Content.Items.Vanity.Excellence;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.SonicTheHedgehog;

public class SonicHeadLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool IsHeadLayer => true;

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Head);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D headTexture = TextureAssets.ArmorHead[EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Head)].Value;
        Rectangle headFrame = headTexture.Frame(1, 3, 0, (int)drawInfo.drawPlayer.eyeHelper.CurrentEyeFrame);

        bool running = drawInfo.drawPlayer.direction == Math.Sign(drawInfo.drawPlayer.velocity.X) && drawInfo.drawPlayer.GetModPlayer<SonicTheHedgehogPlayer>().IsRunning;
        Vector2 headPos = drawInfo.HeadPosition() + new Vector2(4 * Utils.ToInt(running) * drawInfo.drawPlayer.direction, 0);
        headPos.ApplyVerticalOffset(drawInfo);
        Vector2 headVect = drawInfo.headVect + new Vector2(4, 0);

        DrawData headData = new DrawData(headTexture, headPos, headFrame, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, headVect, 1f, drawInfo.playerEffect, 0);
        headData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(headData);
    }
}

public class SonicLegsLayer : PlayerDrawLayer
{
    public static Asset<Texture2D> legsSpeedTexture;

    public override void Load()
    {
        legsSpeedTexture = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/SonicTheHedgehog/Sonic_{EquipType.Legs}_Fast");
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);
    
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Legs);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        bool running = drawInfo.drawPlayer.velocity.Y == 0 && drawInfo.drawPlayer.GetModPlayer<SonicTheHedgehogPlayer>().IsRunning;
        
        Texture2D legsTexture = TextureAssets.ArmorLeg[EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Legs)].Value;
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

        DrawData legData = new DrawData(legsTexture, legPos, legsTexture.Frame(1, 17, 0, legFrame), drawInfo.colorArmorLegs, drawInfo.drawPlayer.legRotation, new Vector2(drawInfo.legVect.X, drawInfo.legVect.Y + originOffY), 1f, drawInfo.playerEffect, 0);
        legData.shader = drawInfo.cLegs;
        drawInfo.DrawDataCache.Add(legData);

        if (running)
        {
            Vector2 fastLegPos = drawInfo.LegsPosition() + drawInfo.legsOffset + new Vector2(0, drawInfo.drawPlayer.gravDir * 2);
            Vector2 fastLegVect = new Vector2(legsSpeedTexture.Width() / 2 + 2 * drawInfo.drawPlayer.direction, drawInfo.legVect.Y + originOffY);

            int fastLegFrame = (int)drawInfo.drawPlayer.GetModPlayer<SonicTheHedgehogPlayer>().runFrame % 7;

            DrawData fastLegData = new DrawData(legsSpeedTexture.Value, fastLegPos, legsSpeedTexture.Frame(1, 7, 0, fastLegFrame), drawInfo.colorArmorLegs, drawInfo.drawPlayer.legRotation, fastLegVect, 1f, drawInfo.playerEffect, 0);
            fastLegData.shader = drawInfo.cLegs;
            drawInfo.DrawDataCache.Add(fastLegData);
        }
    }
}
