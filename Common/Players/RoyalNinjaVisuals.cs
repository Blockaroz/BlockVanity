﻿using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using ParticleEngine;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players
{
    public class RoyalNinjaVisuals : ModPlayer
    {
        public bool active;

        public override void FrameEffects()
        {
            if (!Player.hideMisc[0] && active && !Main.gameMenu & !Main.gamePaused)
            {
                Player.armorEffectDrawShadow = true;

                if (Player.velocity.Length() > 3.2f)
                {
                    ArmorShaderData shader = GameShaders.Armor.GetSecondaryShader(Player.cBody, Player);
                    Color smokeColor = Color.Lerp(Color.Lerp(new Color(66, 77, 99), Color.Indigo, Main.rand.NextFloat(0.66f)), new Color(10, 10, 10, 220), Main.rand.NextFloat());
                    Particle smoke = Particle.NewParticle(Particle.ParticleType<SmokeWisp>(), Player.MountedCenter + Player.velocity * 0.8f - new Vector2(18, Main.rand.Next(-11, 11)).RotatedBy(Player.velocity.ToRotation()), -Player.velocity.RotatedByRandom(0.1f) * 0.05f, smokeColor, Main.rand.NextFloat(0.7f, 1.3f));
                    smoke.shader = shader;
                }
            }
        }

        public override void ResetEffects()
        {
            active = false;
        }
    }
}
