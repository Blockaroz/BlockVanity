using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BlockheadVanity;

public class BlockarozHeadLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(BlockarozHead), EquipType.Head);

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D headTexture = TextureAssets.ArmorHead[drawInfo.drawPlayer.head].Value;
        Vector2 pos = drawInfo.HeadPosition();
        pos.ApplyVerticalOffset(drawInfo);
        Rectangle headFrame = headTexture.Frame(1, 3, 0, (int)drawInfo.drawPlayer.eyeHelper.CurrentEyeFrame);

        DrawData drawData = new DrawData(headTexture, pos, headFrame, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        drawData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(drawData);
    }
}
