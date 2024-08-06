using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.MushroomCow;

public class MushroomCowNeckLayer : PlayerDrawLayer
{
    public static Asset<Texture2D> neckTexture;

    public override void Load()
    {
        neckTexture = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/MushroomCow/MushroomCowHead_Neck");
    }

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.NeckAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
        drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<MushroomCowHead>().Name, EquipType.Head) ||
        drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<GamingMushroomCowHead>().Name, EquipType.Head);

    public override bool IsHeadLayer => false;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 pos = drawInfo.HeadPosition();
        pos.ApplyVerticalOffset(drawInfo);
        DrawData neckData = new DrawData(neckTexture.Value, pos, null, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        neckData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(neckData);
    }
}
