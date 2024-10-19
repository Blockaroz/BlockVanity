using BlockVanity.Content.Items.Vanity.CountChaos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;

public class HellforgedKingHeadGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(HellforgedKingSkull), EquipType.Head);

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 pos = drawInfo.HeadPosition();

        Color drawColor = Color.White with { A = 200 };
        if (drawInfo.shadow > 0f)
            drawColor = Color.DarkOrange with { A = 0 } * (1f - drawInfo.shadow);

        DrawData glowData = new DrawData(HellforgedKingSkull.glowTextureArmor.Value, pos, drawInfo.drawPlayer.bodyFrame, drawColor, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        glowData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(glowData);
    }
}

public class HellforgedKingBodyGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(HellforgedKingGarb), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = HellforgedKingGarb.glowTextureArmor.Value;
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color drawColor = Color.White with { A = 200 };
        if (drawInfo.shadow > 0f)
            drawColor = Color.DarkOrange with { A = 0 } * (1f - drawInfo.shadow);

        DrawData data = new DrawData(glowTexture, position, drawInfo.compTorsoFrame, drawColor, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;

        drawInfo.DrawDataCache.Add(data);
    }
}

public class HellforgedKingArmOnGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(HellforgedKingGarb), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = HellforgedKingGarb.glowTextureArmor.Value;
        Vector2 compOffset = VanityUtils.GetCompositeOffset_FrontArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color drawColor = Color.White with { A = 200 };
        if (drawInfo.shadow > 0f)
            drawColor = Color.DarkOrange with { A = 0 } * (1f - drawInfo.shadow);

        DrawData shoulderData = new DrawData(glowTexture, position, drawInfo.compFrontShoulderFrame, drawColor, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        shoulderData.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(shoulderData);

        if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
            position += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));

        DrawData data = new DrawData(glowTexture, position + compOffset, drawInfo.compFrontArmFrame, drawColor, drawInfo.compositeFrontArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(data);
    }
}

public class HellforgedKingArmOffGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(HellforgedKingGarb), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = HellforgedKingGarb.glowTextureArmor.Value;
        Vector2 compOffset = VanityUtils.GetCompositeOffset_BackArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color drawColor = Color.White with { A = 200 };
        if (drawInfo.shadow > 0f)
            drawColor = Color.DarkOrange with { A = 0 } * (1f - drawInfo.shadow);

        DrawData shoulderData = new DrawData(glowTexture, position, drawInfo.compBackShoulderFrame, drawColor, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        shoulderData.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(shoulderData);

        DrawData data = new DrawData(glowTexture, position + compOffset, drawInfo.compBackArmFrame, drawColor, drawInfo.compositeBackArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(data);
    }
}
