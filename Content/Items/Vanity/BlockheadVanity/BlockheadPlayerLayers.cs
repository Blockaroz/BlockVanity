using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.MushroomCow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BlockheadVanity;

public class BlockheadNeck : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.NeckAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<Blockhead>().Name, EquipType.Head);

    public override bool IsHeadLayer => false;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 pos = drawInfo.HeadPosition();
        pos.ApplyVerticalOffset(drawInfo);

        DrawData neckData = new DrawData(BlockheadHead.neckTexture.Value, pos, null, drawInfo.colorBodySkin, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        neckData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(neckData);
    }
}

public class BlockheadHead : PlayerDrawLayer
{
    public static Asset<Texture2D> headTexture;
    public static Asset<Texture2D> neckTexture;
    public static Asset<Texture2D>[] eyesTexture;

    public override void Load()
    {
        headTexture = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/BlockheadVanity/Blockhead_Head");
        neckTexture = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/BlockheadVanity/Blockhead_Neck");
        eyesTexture = [
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/BlockheadVanity/Blockhead_Eyes0"),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/BlockheadVanity/Blockhead_Eyes1"),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/BlockheadVanity/Blockhead_Eyes2")
            ];
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<Blockhead>().Name, EquipType.Head);

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 pos = drawInfo.HeadPosition();
        pos.ApplyVerticalOffset(drawInfo);
        Rectangle headFrame = headTexture.Value.Frame();
        Rectangle eyeRect = eyesTexture[2].Value.Frame(1, 3, 0, (int)drawInfo.drawPlayer.eyeHelper.CurrentEyeFrame);

        DrawData headData = new DrawData(headTexture.Value, pos, headFrame, drawInfo.colorBodySkin, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        DrawData scleraData = new DrawData(eyesTexture[0].Value, pos, headFrame, drawInfo.colorEyeWhites, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        DrawData irisData = new DrawData(eyesTexture[1].Value, pos, headFrame, drawInfo.colorEyes, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        DrawData eyelidData = new DrawData(eyesTexture[2].Value, pos, eyeRect, drawInfo.colorBodySkin, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);

        headData.shader = drawInfo.cHead;
        scleraData.shader = drawInfo.cHead;
        irisData.shader = drawInfo.cHead;
        eyelidData.shader = drawInfo.cHead;

        drawInfo.DrawDataCache.Add(headData);
        drawInfo.DrawDataCache.Add(scleraData);
        drawInfo.DrawDataCache.Add(irisData);
        drawInfo.DrawDataCache.Add(eyelidData);
    }
}
