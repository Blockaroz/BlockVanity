using BlockVanity.Common.Graphics;
using BlockVanity.Common.Players;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items;

public class ParticlePearl : ModItem
{
    public override string Texture => AllAssets.Textures.Placeholder;

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.useTime = 5;
        Item.useAnimation = 5;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.autoReuse = true;
        Item.channel = true;
        Item.UseSound = SoundID.Item45 with { Pitch = -0.5f, Volume = 0.7f, PitchVariance = 0.5f, MaxInstances = 0 };
    }

    public override Color? GetAlpha(Color lightColor) => (Color.LightSlateGray * 1.5f) with { A = 200 };

    public static Vector2 lastMouse;

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
            lastMouse = Main.MouseWorld;

        return true;
    }

    public override void UseItemFrame(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            int x = (int)(Main.MouseWorld.X / 16);
            int y = (int)(Main.MouseWorld.Y / 16);
            Vector2 velocity = (Main.MouseWorld - lastMouse) * 0.3f + Main.rand.NextVector2Circular(4, 4);

            //MagicSmokeParticle particle = MagicSmokeParticle.pool.RequestParticle();
            //particle.Prepare(Main.MouseWorld, velocity, velocity.ToRotation(), 20, Color.White with { A = 200 }, Color.Orange with { A = 10 }, 0.5f + Main.rand.NextFloat());
            //ParticleEngine.Particles.Add(particle);

            lastMouse = Main.MouseWorld;
        }
    }
}
