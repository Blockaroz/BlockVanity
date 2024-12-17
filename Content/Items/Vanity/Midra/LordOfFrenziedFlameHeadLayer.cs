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

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        PlayerOfFrenziedFlame particlePlayer = drawInfo.drawPlayer.GetModPlayer<PlayerOfFrenziedFlame>();
        if (particlePlayer.IsReady)
        {
            DrawData data = particlePlayer.GetFrenzyTarget();
            data.position = drawInfo.Center - Main.screenPosition;
            data.color = Color.White;
            if (drawInfo.shadow > 0f)
                data.color *= (1f - drawInfo.shadow) * 0.5f;

            data.effect = Main.GameViewMatrix.Effects;
            data.shader = particlePlayer.targetShader;
            drawInfo.DrawDataCache.Add(data);
        }
    }

}
