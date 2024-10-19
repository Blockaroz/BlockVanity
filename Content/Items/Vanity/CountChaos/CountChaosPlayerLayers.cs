using System;
using BlockVanity.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.CountChaos;

public class ChaosFireLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => PlayerDrawLayers.BeforeFirstVanillaLayer;

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        CountChaosPlayer particlePlayer = drawInfo.drawPlayer.GetModPlayer<CountChaosPlayer>();
        if (particlePlayer.IsReady && drawInfo.shadow <= 0f)
        {
            DrawData data = particlePlayer.GetChaosFire();
            data.position = drawInfo.Center - Main.screenPosition;
            data.color = Color.Lerp(Color.White, drawInfo.colorArmorBody, 0.7f);
            data.effect = Main.GameViewMatrix.Effects;
            data.shader = particlePlayer.flameShader;
            drawInfo.DrawDataCache.Add(data);
        }
    }
}

public class CountChaosGownLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D legsTexture = TextureAssets.ArmorLeg[EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs)].Value;

        Vector2 position = drawInfo.LegsPosition() + new Vector2(0, drawInfo.drawPlayer.gravDir > 0 ? -2 : -26);
        Vector2 origin = new Vector2(drawInfo.legVect.X, 28 + 12 * drawInfo.drawPlayer.gravDir);

        position.ApplyVerticalOffset(drawInfo);
        float rotationExtra = Utils.GetLerpValue(1, drawInfo.drawPlayer.accRunSpeed, Math.Abs(drawInfo.drawPlayer.velocity.X), true) * drawInfo.drawPlayer.direction * drawInfo.drawPlayer.gravDir * 0.1f;
        float originOffY = drawInfo.isSitting ? -4 : 0;
        DrawData data = new DrawData(legsTexture, position + new Vector2(0, originOffY), legsTexture.Frame(), drawInfo.colorArmorLegs, drawInfo.drawPlayer.legRotation + rotationExtra, origin, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cLegs;
        drawInfo.DrawDataCache.Add(data);

    }
}

public class CountChaosBodyGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = CountChaosCuirass.glowTextureArmor.Value;
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color color = Color.Lerp(Color.White, drawInfo.colorArmorBody, 0.5f) with { A = 0 } * 0.8f * (1f - drawInfo.shadow);
        DrawData data = new DrawData(glowTexture, position, drawInfo.compTorsoFrame, color, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;

        drawInfo.DrawDataCache.Add(data);
    }
}

public class CountChaosArmOnGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = CountChaosCuirass.glowTextureArmor.Value;
        Vector2 compOffset = VanityUtils.GetCompositeOffset_FrontArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color color = Color.Lerp(Color.White, drawInfo.colorArmorBody, 0.5f) with { A = 0 } * 0.8f * (1f - drawInfo.shadow);
        DrawData shoulderData = new DrawData(glowTexture, position, drawInfo.compFrontShoulderFrame, color, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        shoulderData.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(shoulderData);

        if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
            position += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));

        DrawData data = new DrawData(glowTexture, position + compOffset, drawInfo.compFrontArmFrame, color, drawInfo.compositeFrontArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(data);
    }
}

public class CountChaosArmOffGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Texture2D glowTexture = CountChaosCuirass.glowTextureArmor.Value;
        Vector2 compOffset = VanityUtils.GetCompositeOffset_BackArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        Color color = Color.Lerp(Color.White, drawInfo.colorArmorBody, 0.5f) with { A = 0 } * 0.8f * (1f - drawInfo.shadow);
        DrawData shoulderData = new DrawData(glowTexture, position, drawInfo.compBackShoulderFrame, color, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        shoulderData.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(shoulderData);

        DrawData data = new DrawData(glowTexture, position + compOffset, drawInfo.compBackArmFrame, color, drawInfo.compositeBackArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(data);
    }
}
