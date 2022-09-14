using BlockVanity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players
{
    public class TrailItemPlayer : ModPlayer
    {
        public override void PostUpdateRunSpeeds()
        {
            for (int i = 0; i < Player.armor.Length; i++)
            {
                if (Player.armor[i].ModItem is not ITrailItem)
                    continue;
                else
                {
                    ArmorShaderData dye = GameShaders.Armor.GetShaderFromItemId(Player.dye[i % 10].type);
                    (Player.armor[i].ModItem as ITrailItem).UpdateParticleEffect(Player, i, dye);
                }
            }
        }
    }
}
