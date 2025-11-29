using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Midra;

public class FrenziedFlameHeadEffectLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(AshenHead), EquipType.Head);

    public override bool IsHeadLayer => false;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.headOnlyRender)
        {

        }

        // Draw the animated thing here
    }
}

public class FrenziedFlameBackHeadEffectLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(AshenHead), EquipType.Head);

    public override bool IsHeadLayer => false;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        PlayerOfFrenziedFlame particlePlayer = drawInfo.drawPlayer.GetModPlayer<PlayerOfFrenziedFlame>();

        DrawData data = particlePlayer.GetFrenzyTarget();
        data.position = drawInfo.HeadPosition() + new Vector2(2 * drawInfo.drawPlayer.direction, (drawInfo.drawPlayer.gravDir < 0 ? 11 : 0) + -8 * drawInfo.drawPlayer.gravDir);
        data.position.ApplyVerticalOffset(drawInfo);
        data.color = Color.White;
        data.effect = Main.GameViewMatrix.Effects;
        data.shader = drawInfo.cHead;
        // drawInfo.DrawDataCache.Add(data);
    }
}