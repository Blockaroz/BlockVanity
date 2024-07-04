using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MushroomCowVisuals : ModPlayer
{
    public bool red;
    public bool brown;
    public bool glowing;
    private bool AnyCow { get => red || brown || glowing; }

    public override void FrameEffects()
    {
        bool rightSpeed = Player.velocity.Length() < 4f;
        ArmorShaderData shader = GameShaders.Armor.GetSecondaryShader(Player.cBody, Player);
        if (!Player.hideMisc[0] && rightSpeed && !Main.gameMenu & !Main.gamePaused && AnyCow)
        {
            Vector2 mushPos = Player.Bottom + Main.rand.NextVector2Circular(40, 30);
            bool doMushroom = true;
            const int maxTiles = 5;
            for (int i = -2; i < maxTiles; i++)
            {
                Point mushTile = new Point((int)(mushPos.X / 16f), (int)(mushPos.Y / 16f));
                if (WorldGen.ActiveAndWalkableTile(mushTile.X, mushTile.Y + i))
                {
                    mushPos.Y = (mushTile.Y + i) * 16f;
                    break;
                }

                if (i >= maxTiles - 1)
                    doMushroom = false;
            }

            float scale = Utils.GetLerpValue(120, 40, (mushPos - Player.Center).Length()) * 1.11f;

            if (red && doMushroom && Main.rand.NextBool(15))
            {
            }
            if (brown && doMushroom)
            {
                if (Main.rand.NextBool(20))
                {
                }
                if (Main.rand.NextBool(70))
                {
                    Dust shroomDust = Dust.NewDustPerfect(Player.Center + Main.rand.NextVector2Circular(12, 6), DustID.Electric, -Vector2.UnitY.RotatedByRandom(1f) * 2f, 0, Color.White, 0.5f);
                    shroomDust.noGravity = true;
                    shroomDust.shader = shader;
                }
            }
            if (glowing)
            {
                if (doMushroom && Main.rand.NextBool(18))
                {
                }

                if (Main.rand.NextBool(80))
                {
                    Dust shroomDust = Dust.NewDustPerfect(Player.Bottom + Main.rand.NextVector2Circular(24, 2), DustID.GlowingMushroom, -Vector2.UnitY.RotatedByRandom(1f), 0, Color.White, 0.8f);
                    shroomDust.shader = shader;
                }
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
