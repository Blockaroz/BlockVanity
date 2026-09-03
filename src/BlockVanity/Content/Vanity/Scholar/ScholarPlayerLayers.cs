using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Scholar;

public class ScholarHoodEyesLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(ScholarHoodItem), EquipType.Head);

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var eyesTexture = Assets.Textures.Content.Vanity.Scholar.ScholarHood_Item_Eyes.Asset;

        float colorStrength = MathHelper.Clamp(drawInfo.drawPlayer.eyeColor.R + drawInfo.drawPlayer.eyeColor.G + drawInfo.drawPlayer.eyeColor.B, 0, 255) / 255f;
        Color eyeColor = drawInfo.drawPlayer.eyeColor * Utils.GetLerpValue(0.4f, 0.6f, colorStrength, true);

        Rectangle eyeRect = eyesTexture.Frame(1, 3, 0, (int)drawInfo.drawPlayer.eyeHelper.CurrentEyeFrame);
        Vector2 pos = drawInfo.HeadPosition();
        pos.ApplyVerticalOffset(drawInfo);

        DrawData eyeData = new DrawData(eyesTexture.Value, pos, eyeRect, eyeColor, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        eyeData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(eyeData);
    }
}

public class ScholarCloakCoatLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmorLongCoat);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(ScholarCloak), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var coatTexture = Assets.Textures.Content.Vanity.Scholar.ScholarCloak_Item_Coat.Asset;

        if (drawInfo.isSitting)
        {
            VanityUtils.DrawSittingLongCoats(ref drawInfo, coatTexture.Value, drawInfo.colorArmorBody, drawInfo.cBody);
            return;
        }

        DrawData item = new DrawData(coatTexture.Value, new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.legFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect, drawInfo.drawPlayer.legFrame, drawInfo.colorArmorBody, drawInfo.drawPlayer.legRotation, drawInfo.legVect, 1f, drawInfo.playerEffect);
        item.shader = drawInfo.cBody;
        drawInfo.DrawDataCache.Add(item);
    }
}