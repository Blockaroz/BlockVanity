using BlockVanity.Content.Particles;
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
                Player.armorEffectDrawOutlines = true;
                Player.armorEffectDrawShadow = true;

                if (Player.velocity.Length() > 2.7f)
                {
                    Color smokeColor = Color.Lerp(Color.Lerp(new Color(66, 77, 99), Color.Indigo, Main.rand.NextFloat(0.66f)), Color.Black * 0.8f, Main.rand.NextFloat());
                    Particle smoke = Particle.NewParticle(Particle.ParticleType<SmokeWisp>(), Player.Center + Player.velocity * 0.8f - new Vector2(18, Main.rand.Next(-12, 12)).RotatedBy(Player.velocity.ToRotation()), -Player.velocity.RotatedByRandom(0.5f) * 0.2f, smokeColor, Main.rand.NextFloat(0.5f, 1.2f));
                    smoke.shader = GameShaders.Armor.GetSecondaryShader(Player.cBody, Player);
                }
            }
        }

        public override void ResetEffects()
        {
            active = false;
        }
    }
}
