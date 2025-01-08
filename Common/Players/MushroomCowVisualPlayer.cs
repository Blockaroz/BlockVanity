using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MushroomCowVisualPlayer : ModPlayer
{
    public bool red;
    private bool AnyEffect => red;

    //public override void FrameEffects()
    //{
    //    if (!Main.gamePaused && !Main.gameMenu && AreaEffectsToggle.IsActive(Player) && AnyEffect)
    //    {
    //        bool fast = Math.Abs(Player.velocity.X) > Player.maxRunSpeed * 0.9f && Main.rand.NextBool();
    //        if (Main.rand.NextBool(20) || fast)
    //        {
    //            Vector2 mushPos = Player.Top;
    //            Vector2 mushVel = Main.rand.NextVector2CircularEdge(100, 150);
    //            Vector2 mushCollision = Vector2.Zero;
    //            Collision.AimingLaserScan(mushPos, mushPos + mushVel, 24, 10, out mushCollision, out float[] samples);
    //            float sum = 0;
    //            for (int i = 0; i < samples.Length; i++)
    //                sum += samples[i];
    //            sum /= samples.Length;

    //            mushCollision = mushCollision.SafeNormalize(Vector2.Zero) * sum;
    //            mushCollision = Collision.AnyCollision(mushPos - Vector2.One, mushCollision, 2, 2);

    //            mushPos += mushCollision.SafeNormalize(Vector2.Zero) * sum - Vector2.UnitY * 4;

    //            bool placeMushroom = WorldGen.SolidOrSlopedTile((int)Math.Floor(mushPos.X / 16), (int)Math.Floor(mushPos.Y / 16));

    //            if (red && placeMushroom)
    //            {
    //                float scale = Utils.GetLerpValue(120, 40, (mushPos - Player.Top).Length()) * 2f * Main.rand.NextFloat(0.8f, 1.2f);
    //                float rotation = Collision.AnyCollision(Player.Top - Vector2.One, mushVel, 2, 2).ToRotation() - MathHelper.PiOver2 + Main.rand.NextFloat(-0.15f, 0.15f);

    //                int shaderId = Main.rand.Next([Player.cHead, Player.cBody, Player.cLegs]);
    //                ArmorShaderData shaderData = GameShaders.Armor.GetSecondaryShader(shaderId, Player);

    //                int timeLeft = fast ? Main.rand.Next(50, 100) : Main.rand.Next(200, 350);
    //                ParticleEngine.particles.NewParticle(new GrowingMushroomParticle(0, timeLeft, shaderData), mushPos, Vector2.Zero, rotation, scale);
    //            }
    //        }
    //    }
    //}

    public override void ResetEffects()
    {
        red = false;
    }
}
