using System;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Particles;

public struct GrowingMushroomParticle : IShaderParticleData
{
    public int maxTime;
    public int timeLeft;
    public int frame;
    private int style;
    private ArmorShaderData shaderData;

    public bool ShaderEnabled => shaderData != null;

    public GrowingMushroomParticle(int style, int timeLeft = 200, ArmorShaderData shaderData = null)
    {
        maxTime = timeLeft;
        this.shaderData = shaderData;
        this.style = style;
    }

    public void OnSpawn(Particle particle)
    {
        particle.velocity = Vector2.Zero;

        switch (style)
        {
            default:
            case 0:
                frame = Main.rand.Next(7);
                break;
        }
    }

    public void Update(Particle particle)
    {
        int tileX = (int)Math.Floor(particle.position.X / 16);
        int tileY = (int)Math.Floor(particle.position.Y / 16);
        if (!WorldGen.InWorld(tileX, tileY))
        {
            particle.active = false;
            return;
        }

        Tile tile = Main.tile[tileX, tileY];

        if (timeLeft < 3)
        {
            Dust d = Main.dust[WorldGen.KillTile_MakeTileDust(tileX, tileY, tile)];
            d.scale *= 0.8f;
            d.noGravity = Main.rand.NextBool();
            d.velocity *= 0.2f;
        }

        if (timeLeft++ > maxTime)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[2].Value;

        float drawscale = Easing.BackOut(Utils.GetLerpValue(0, 25, timeLeft, true)) * MathF.Sqrt(Utils.GetLerpValue(maxTime, maxTime - 50, timeLeft, true)) * (1f + MathF.Sin(timeLeft / 20f) * 0.05f);
        Rectangle drawFrame = texture.Frame(7, 1, frame, style);
        Color drawColor = Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16));
        DrawData drawData = new DrawData(texture, particle.position - anchorPosition, drawFrame, drawColor, particle.rotation, drawFrame.Size() * new Vector2(0.5f, 1f), drawscale, 0, 0);
        
        if (shaderData != null)
            shaderData.Apply(null, drawData);

        drawData.Draw(spriteBatch);
    }
}
