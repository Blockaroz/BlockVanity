﻿using BlockVanity.Common.Graphics;
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
            Vector2 velocity = (Main.MouseWorld - lastMouse) * 0.4f;

            PhysicalSparkParticle particle = PhysicalSparkParticle.pool.RequestParticle();
            particle.Prepare(Main.MouseWorld, velocity, Vector2.UnitY, Color.White with { A = 10 }, (Color.Green * 0.5f) with { A = 50 }, 1f, true);
            ParticleEngine.Particles.Add(particle);
            lastMouse = Main.MouseWorld;
        }
    }
}
