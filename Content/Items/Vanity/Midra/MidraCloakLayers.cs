using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Midra;

public class MidraCloakBodyLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(MidraCloak), EquipType.Back);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D texture = MidraCloak.cloakBodyTexture.Value;
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        DrawData data = new DrawData(texture, position, drawInfo.compTorsoFrame, drawInfo.colorArmorBody, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.cBack;
        drawInfo.DrawDataCache.Add(data);
    }
}

public class MidraCloakArmLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(MidraCloak), EquipType.Back);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D texture = MidraCloak.cloakBodyTexture.Value;

        Vector2 compOffset = VanityUtils.GetCompositeOffset_FrontArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
            position += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));

        DrawData data = new DrawData(texture, position + compOffset, drawInfo.compFrontArmFrame, drawInfo.colorArmorBody, drawInfo.compositeFrontArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBack;
        drawInfo.DrawDataCache.Add(data);
    }
}

public class MidraCloakCapeLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(MidraCloak), EquipType.Back);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D texture = MidraCloak.cloakCapeTexture.Value;

    }
}
