using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.MushroomCow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Scholar;

public class ScholarHoodEyesLayer : PlayerDrawLayer
{
    public static SlowAsset<Texture2D> eyesTexture;

    public override void Load()
    {
        eyesTexture = new SlowAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/Scholar/ScholarHood_Eyes");
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<ScholarHood>().Name, EquipType.Head);

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        float colorStrength = MathHelper.Clamp(drawInfo.drawPlayer.eyeColor.R + drawInfo.drawPlayer.eyeColor.G + drawInfo.drawPlayer.eyeColor.B, 0, 255) / 255f;
        Color eyeColor = drawInfo.drawPlayer.eyeColor * Utils.GetLerpValue(0.4f, 0.6f, colorStrength, true);

        Rectangle eyeRect = eyesTexture.Value.Frame(1, 3, 0, (int)drawInfo.drawPlayer.eyeHelper.CurrentEyeFrame);
        Vector2 pos = drawInfo.HeadPosition();
        pos.ApplyVerticalOffset(drawInfo);

        DrawData eyeData = new DrawData(eyesTexture.Value, pos, eyeRect, eyeColor, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        eyeData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(eyeData);
    }
}
