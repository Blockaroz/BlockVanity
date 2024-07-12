using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Myrtle;

public class FishSkinTailLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Tails);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        FishSkinPlayer fishPlayer = drawInfo.drawPlayer.GetModPlayer<FishSkinPlayer>();
        if (fishPlayer.tailRotations != null)
        {
            Texture2D tailTexture = fishPlayer.SkinTextures[(int)ReskinPlayer.SkinID.Tail].Value;
            int originX = drawInfo.drawPlayer.direction > 0 ? 1 : 0;
            Rectangle upperTailFrame = tailTexture.Frame(3, 1, 2, 0);
            Rectangle midTailFrame = tailTexture.Frame(3, 1, 1, 0);
            Rectangle lowerTailFrame = tailTexture.Frame(3, 1, 0, 0);

            Vector2 upperTailPos = drawInfo.BodyPosition() + new Vector2(0, (8 + Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height].Y) * drawInfo.drawPlayer.gravDir);
            Vector2 midTailPos = upperTailPos + new Vector2(-10 * drawInfo.drawPlayer.direction, 4 * drawInfo.drawPlayer.gravDir).RotatedBy(fishPlayer.tailRotations[0]);
            Vector2 lowerTailPos = midTailPos + new Vector2(-8 * drawInfo.drawPlayer.direction, -6 * drawInfo.drawPlayer.gravDir).RotatedBy(fishPlayer.tailRotations[1]);

            DrawData tailData = new DrawData(tailTexture, upperTailPos, upperTailFrame, drawInfo.colorLegs, fishPlayer.tailRotations[0], new Vector2(originX * upperTailFrame.Width, 14), 1f, drawInfo.playerEffect, 0);
            drawInfo.DrawDataCache.Add(tailData);            
            tailData = new DrawData(tailTexture, midTailPos, midTailFrame, drawInfo.colorLegs, fishPlayer.tailRotations[1], new Vector2(originX * midTailFrame.Width, 14 + 4 * drawInfo.drawPlayer.gravDir), 1f, drawInfo.playerEffect, 0);
            drawInfo.DrawDataCache.Add(tailData);           
            tailData = new DrawData(tailTexture, lowerTailPos, lowerTailFrame, drawInfo.colorLegs, fishPlayer.tailRotations[2], new Vector2(originX * lowerTailFrame.Width, 14 - 2 * drawInfo.drawPlayer.gravDir), 1f, drawInfo.playerEffect, 0);
            drawInfo.DrawDataCache.Add(tailData);

        }
    }
}
