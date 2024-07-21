using System;
using BlockVanity.Common.Graphics;
using BlockVanity.Common.Graphics.ParticleRendering;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MushroomCowVisualPlayer : ModPlayer
{
    public int mushCount;

    public bool red;
    public int cMushroom;
    private bool AnyEffect => red;

    public override void FrameEffects()
    {
        if (!Main.gamePaused && !Main.gameMenu && AreaEffectsToggle.IsActive(Player) && AnyEffect)
        {
            if (Main.rand.NextBool(10) || (Math.Abs(Player.velocity.X) > Player.maxRunSpeed * 0.9f && Main.rand.NextBool()))
            {
                Vector2 mushPos = Player.MountedCenter + Player.velocity;
                Vector2 mushVel = Main.rand.NextVector2CircularEdge(70, 110);
                Vector2 mushCollision = Collision.AnyCollision(mushPos - Vector2.One * 4, mushVel, 8, 8);

                mushPos += mushCollision;

                bool placeMushroom = WorldGen.SolidOrSlopedTile((int)Math.Floor(mushPos.X / 16), (int)Math.Floor(mushPos.Y / 16));

                if (red && placeMushroom)
                {
                    float scale = Utils.GetLerpValue(120, 40, (mushPos - Player.Center).Length()) * 2f * Main.rand.NextFloat(0.8f, 1.2f);
                    float rotation = mushCollision.ToRotation() - MathHelper.PiOver2 + Main.rand.NextFloat(-0.15f, 0.15f);
                    ParticleEngine.particles.NewParticle(new GrowingMushroomParticle(0, Main.rand.Next(150, 250)), mushPos, Vector2.Zero, rotation, scale);
                }
            }
        }
    }

    public override void ResetEffects()
    {
        red = false;
        mushCount = 0;
    }
}
