using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Players;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BlockVanity.Content.Items.Vanity.Midra;

public class FrenziedFlameHeadLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ProjectileOverArm);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(EyeOfFrenziedFlame), EquipType.Head);

    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        PlayerOfFrenziedFlame particlePlayer = drawInfo.drawPlayer.GetModPlayer<PlayerOfFrenziedFlame>();

        if (particlePlayer.IsReady && drawInfo.shadow <= 0f)
        {
            DrawData data = particlePlayer.GetFrenzyTargetFront();
            data.position = drawInfo.Center.Floor() + new Vector2(drawInfo.drawPlayer.direction, -16 * drawInfo.drawPlayer.gravDir + drawInfo.mountOffSet / 2f) - Main.screenPosition;
            data.color = Color.White;
            data.effect = Main.GameViewMatrix.Effects;
            data.shader = particlePlayer.targetShader;
            drawInfo.DrawDataCache.Add(data);
        }
    }
}

public class FrenziedFlameBackLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Wings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(EyeOfFrenziedFlame), EquipType.Head);

    public override bool IsHeadLayer => false;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        drawInfo.colorArmorHead = Color.White;
        if (drawInfo.shadow > 0f)
            drawInfo.colorArmorHead = Color.Transparent;

        PlayerOfFrenziedFlame particlePlayer = drawInfo.drawPlayer.GetModPlayer<PlayerOfFrenziedFlame>();
        if (particlePlayer.IsReady && drawInfo.shadow <= 0f)
        {
            DrawData data = particlePlayer.GetFrenzyTarget();
            data.position = drawInfo.Center.Floor() + new Vector2(drawInfo.drawPlayer.direction, -18 * drawInfo.drawPlayer.gravDir + drawInfo.mountOffSet / 2f) - Main.screenPosition;
            data.color = Color.White;
            data.effect = Main.GameViewMatrix.Effects;
            data.shader = particlePlayer.targetShader;
            drawInfo.DrawDataCache.Add(data);
        }
    }
}
