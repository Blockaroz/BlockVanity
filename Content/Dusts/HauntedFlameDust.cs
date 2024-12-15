using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dusts;
public class HauntedFlameDust : ModDust
{
    public override void OnSpawn(Dust dust)
    {
        dust.frame = new Rectangle(32 * Main.rand.Next(3), 0, 32, 32);
        dust.scale += Main.rand.NextFloat(0.5f);
        dust.fadeIn += Main.rand.NextFloat(1.5f);
    }

    public override bool Update(Dust dust)
    {
        dust.velocity *= 0.95f;
        dust.velocity += (Main.rand.NextVector2Circular(1, 1) - Vector2.UnitY * 0.1f) / (dust.scale * 2f + 1f);
        if (!dust.noLightEmittence)
            Lighting.AddLight(dust.position, dust.color.ToVector3() * new Vector3(0.5f, 0.05f, 0.1f) * dust.scale);

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
