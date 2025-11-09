using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common;

public class NeckPlayerLayer : PlayerDrawLayer
{
    public static Dictionary<int, Asset<Texture2D>> neckTextures;

    public static void AddNeck(ModItem item)
    {
        neckTextures ??= new Dictionary<int, Asset<Texture2D>>();
        neckTextures.Add(item.Item.headSlot, ModContent.Request<Texture2D>($"{item.Texture}_Neck"));
    }

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.NeckAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => neckTextures.ContainsKey(drawInfo.drawPlayer.head);

    public override bool IsHeadLayer => false;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D neckTexture = neckTextures[drawInfo.drawPlayer.head].Value;
        Vector2 pos = drawInfo.HeadPosition();
        pos.ApplyVerticalOffset(drawInfo);
        DrawData neckData = new DrawData(neckTexture, pos, null, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        neckData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(neckData);
    }
}