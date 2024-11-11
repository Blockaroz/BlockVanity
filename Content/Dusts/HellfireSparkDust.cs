using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dusts;

public class HellfireSparkDust : ModDust
{
    public override void OnSpawn(Dust dust)
    {
        dust.fadeIn = Math.Max(35, dust.fadeIn) + Main.rand.Next(10);
        dust.color *= Main.rand.NextFloat(0.7f, 1.1f);
    }

    public override bool Update(Dust dust)
    {
        dust.position += dust.velocity;
        dust.velocity *= 0.97f;
        dust.velocity.Y -= Main.rand.NextFloat(-0.5f, 2f) * 0.03f;
        dust.rotation = MathF.Round(dust.velocity.ToRotation() / MathHelper.TwoPi * 8f) / 8f * MathHelper.TwoPi;

        if (dust.fadeIn > 25)
            dust.fadeIn -= 5f / dust.scale;
        else
            dust.fadeIn *= 0.89f;

        if (!dust.noLight)
            Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.1f * Utils.GetLerpValue(0f, 1f, dust.fadeIn, true));

        if (dust.fadeIn < 0.1f)
            dust.active = false;

        return false;
    }

    public override bool PreDraw(Dust dust)
    {
        Texture2D texture = Texture2D.Value;
        Texture2D glow = AllAssets.Textures.Pixel.Value;

        Color drawColor = Color.White with { A = 10 } * Utils.GetLerpValue(2f, 8f, dust.fadeIn, true);
        Color glowColor = dust.color with { A = 10 } * Utils.GetLerpValue(0f, 25f, dust.fadeIn, true);

        Vector2 particlePos = new Vector2((int)Math.Round(dust.position.X / 2f) * 2f, (int)Math.Round(dust.position.Y / 2f) * 2f) - Main.screenPosition;
        Vector2 unRotatedVel = dust.velocity.RotatedBy(-dust.rotation);
        float xS = Math.Abs(unRotatedVel.X) * 3f + dust.scale;
        float yS = Math.Abs(unRotatedVel.Y) * 3f + dust.scale;
        Vector2 stretch = new Vector2(Math.Max(xS - yS * 0.5f, 2f), Math.Max(yS - xS * 0.1f, 2f)) * 0.5f;
        DrawData drawData = new DrawData(texture, particlePos, texture.Frame(), drawColor, dust.rotation, texture.Size() * 0.5f, stretch, 0, 0);
        DrawData glowData = new DrawData(glow, particlePos, glow.Frame(), glowColor, dust.rotation, glow.Size() * 0.5f, stretch, 0, 0);

        dust.shader?.Apply(null, drawData);
        Main.EntitySpriteDraw(drawData);
        dust.shader?.Apply(null, glowData);
        Main.EntitySpriteDraw(glowData);

        return false;
    }
}
