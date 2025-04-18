﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dusts;
public class HauntedFlameDust : ModDust
{
    public override void OnSpawn(Dust dust)
    {
        dust.frame = new Rectangle(32 * Main.rand.Next(3), 0, 32, 32);
        dust.scale += Main.rand.NextFloat(0.5f);
        dust.fadeIn += Main.rand.NextFloat();
    }

    public override bool Update(Dust dust)
    {
        dust.velocity *= 0.95f;
        dust.velocity += (Main.rand.NextVector2Circular(1, 1) - Vector2.UnitY * 0.1f) / (dust.scale * 2f + 1f);
        if (!dust.noLightEmittence)
        {
            Lighting.AddLight(dust.position, new Vector3(0.1f, 0.2f, 0.4f) * dust.scale);
        }

        return true;
    }

    public override bool PreDraw(Dust dust)
    {
        DrawData drawData = new DrawData(Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.color, dust.rotation, dust.frame.Size() * 0.5f, dust.scale, 0, 0);
        dust.shader?.Apply(null, drawData);
        Main.EntitySpriteDraw(drawData);

        return false;
    }
}