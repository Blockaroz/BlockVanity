using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.CountChaos;

public class ChaosParticleLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => PlayerDrawLayers.BeforeFirstVanillaLayer;

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
        drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosCuirass), EquipType.Body) ||
        drawInfo.drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        CountChaosPlayer particlePlayer = drawInfo.drawPlayer.GetModPlayer<CountChaosPlayer>();

        if (particlePlayer.IsReady() && drawInfo.shadow <= 0f && !drawInfo.hideEntirePlayer)
        {
            DrawData data = particlePlayer.GetChaosParticleTarget();
            data.position = drawInfo.Center.Floor() + new Vector2(0, drawInfo.mountOffSet / 2f) - Main.screenPosition;
            data.color = Color.Lerp(Color.White, drawInfo.colorArmorBody, 0.5f);
            data.effect = Main.GameViewMatrix.Effects;
            data.shader = particlePlayer.targetShader;
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
        float xOffset = Utils.GetLerpValue(1, drawInfo.drawPlayer.accRunSpeed, Math.Abs(drawInfo.drawPlayer.velocity.X), true) * drawInfo.drawPlayer.direction;
        float yOffset = Math.Clamp(-drawInfo.drawPlayer.velocity.Y * 0.1f * drawInfo.drawPlayer.gravDir, -0.1f, 3) * drawInfo.drawPlayer.gravDir;
        float originOffY = (drawInfo.isSitting ? -4 : 0);

        DrawData data = new DrawData(legsTexture, position + new Vector2(-xOffset * 1.5f, originOffY + yOffset * 2), legsTexture.Frame(2, 1, 1), drawInfo.colorArmorLegs, drawInfo.drawPlayer.legRotation + xOffset * 0.05f * drawInfo.drawPlayer.gravDir, origin, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cLegs;
        drawInfo.DrawDataCache.Add(data);

        data = new DrawData(legsTexture, position + new Vector2(-xOffset * 0.5f, originOffY + yOffset), legsTexture.Frame(2, 1, 0), drawInfo.colorArmorLegs, drawInfo.drawPlayer.legRotation, origin, 1f, drawInfo.playerEffect, 0);
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
        if (!drawInfo.hideCompositeShoulders)
        {
            DrawData shoulderData = new DrawData(glowTexture, position, drawInfo.compFrontShoulderFrame, color, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
            shoulderData.shader = drawInfo.drawPlayer.cBody;
            drawInfo.DrawDataCache.Add(shoulderData);
        }

        if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
        {
            position += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
        }

        DrawData data = new DrawData(glowTexture, position + compOffset, drawInfo.compFrontArmFrame, color, drawInfo.compositeFrontArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBody;
        drawInfo.DrawDataCache.Add(data);
    }
}

public class CountChaosArmOffGlowLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Skin);

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