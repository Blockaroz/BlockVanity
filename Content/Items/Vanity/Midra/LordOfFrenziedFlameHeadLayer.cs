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
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.FrozenOrWebbedDebuff);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => false;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        PlayerOfFrenziedFlame particlePlayer = drawInfo.drawPlayer.GetModPlayer<PlayerOfFrenziedFlame>();
        if (particlePlayer.IsReady && drawInfo.shadow <= 0f)
        {
            DrawData data = particlePlayer.GetFrenzyTarget();
            data.position = drawInfo.Center.Floor() - Main.screenPosition;
            data.color = Color.White;
            data.effect = Main.GameViewMatrix.Effects;
            data.shader = particlePlayer.targetShader;
            drawInfo.DrawDataCache.Add(data);
        }
    }

}
