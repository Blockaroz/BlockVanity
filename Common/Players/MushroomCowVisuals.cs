using System;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using ParticleEngine;

namespace BlockVanity.Common.Players
{
    public class MushroomCowVisuals : ModPlayer
    {
        public bool red;
        public bool brown;
        public bool glowing;

        public override void FrameEffects()
        {
            if (!Player.hideMisc[0] && Player.velocity.Length() < 2f && !Main.gameMenu & !Main.gamePaused)
            {
                Vector2 mushPos = Player.Center + Main.rand.NextVector2CircularEdge(12, 12);
                bool doMushroom = true;
                //const int maxTiles = 3;
                //for (int i = -2; i < maxTiles; i++)
                //{
                //    Point mushTile = new Point((int)(mushPos.X / 16f), (int)(mushPos.Y / 16f));
                //    if (WorldGen.ActiveAndWalkableTile(mushTile.X, mushTile.Y + i))
                //    {
                //        mushPos.Y = mushTile.Y * 16f;
                //        break;
                //    }

                //    if (i >= maxTiles - 1)
                //        doMushroom = false;
                //}

                if (doMushroom)
                {
                    if (red && Main.rand.NextBool(15))
                        Particle.NewParticle(Particle.ParticleType<Content.Particles.Mushrooms.MushroomTintable>(), mushPos, Vector2.Zero, Color.Red, 1f);

                    if (brown)
                    {
                        if (Main.rand.NextBool(15))
                            Particle.NewParticle(Particle.ParticleType<Content.Particles.Mushrooms.MushroomTintable>(), mushPos, Vector2.Zero, Color.Peru, 1f);

                        if (Main.rand.NextBool(40))
                            Dust.NewDustPerfect(Player.Center + Main.rand.NextVector2Circular(12, 15), DustID.Electric, -Vector2.UnitY.RotatedByRandom(1.5f), 0, Color.White, Main.rand.NextFloat(0.7f));
                    }
                    if (glowing && Main.rand.NextBool(1))
                        Particle.NewParticle(Particle.ParticleType<Content.Particles.Mushrooms.MushroomTintable>(), mushPos, Vector2.Zero, Color.Blue, 1f);
                }
            }
        }

        public override void ResetEffects()
        {
            red = false;
            brown = false;
            glowing = false;
        }
    }
}
