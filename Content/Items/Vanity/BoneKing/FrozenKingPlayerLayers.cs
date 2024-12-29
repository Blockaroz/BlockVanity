using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing;

public class FrozenKingHeadGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(FrozenKingSkull), EquipType.Head);

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 pos = drawInfo.HeadPosition();

        Color drawColor = Color.Coral with { A = 60 } * (1f - drawInfo.shadow * 0.5f);

        DrawData glowData = new DrawData(FrozenKingSkull.glowTextureArmor.Value, pos, drawInfo.drawPlayer.bodyFrame, drawColor, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
        glowData.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(glowData);
    }
}

public class FrozenKingBodyGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(FrozenKingGarb), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = FrozenKingGarb.glowTextureArmor.Value;
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color drawColor = Color.Coral with { A = 60 } * (1f - drawInfo.shadow * 0.5f);

        DrawData data = new DrawData(glowTexture, position, drawInfo.compTorsoFrame, drawColor, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;

        drawInfo.DrawDataCache.Add(data);
    }
}

public class FrozenKingArmOnGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(FrozenKingGarb), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = FrozenKingGarb.glowTextureArmor.Value;
        Vector2 compOffset = VanityUtils.GetCompositeOffset_FrontArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color drawColor = Color.Coral with { A = 60 } * (1f - drawInfo.shadow * 0.5f);

        DrawData shoulderData = new DrawData(glowTexture, position, drawInfo.compFrontShoulderFrame, drawColor, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        shoulderData.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(shoulderData);

        if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
            position += new Vector2(!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally) ? 1 : -1, !drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically) ? 1 : -1);

        DrawData data = new DrawData(glowTexture, position + compOffset, drawInfo.compFrontArmFrame, drawColor, drawInfo.compositeFrontArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(data);
    }
}

public class FrozenKingArmOffGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(FrozenKingGarb), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = FrozenKingGarb.glowTextureArmor.Value;
        Vector2 compOffset = VanityUtils.GetCompositeOffset_BackArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color drawColor = Color.Coral with { A = 60 } * (1f - drawInfo.shadow * 0.5f);

        DrawData shoulderData = new DrawData(glowTexture, position, drawInfo.compBackShoulderFrame, drawColor, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        shoulderData.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(shoulderData);

        DrawData data = new DrawData(glowTexture, position + compOffset, drawInfo.compBackArmFrame, drawColor, drawInfo.compositeBackArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(data);
    }
}
