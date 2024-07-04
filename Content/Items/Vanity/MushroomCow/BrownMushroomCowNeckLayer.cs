﻿using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.MushroomCow;

public class BrownMushroomCowNeckLayer : PlayerDrawLayer
{
    public static SlowAsset<Texture2D> neckTexture;

    public override void Load()
    {
        neckTexture = new SlowAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/MushroomCow/BrownMushroomCowHead_Neck");
    }

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.NeckAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
        drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<BrownMushroomCowHead>().Name, EquipType.Head) ||
        drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, ModContent.GetInstance<CoolBrownMushroomCowHead>().Name, EquipType.Head);

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
