using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BlockVanity.Content.Tiles;

public class Stylotron : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileLighted[Type] = true;
        TileID.Sets.HasOutlines[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
        TileObjectData.newTile.Width = 4;
        TileObjectData.newTile.Height = 5;
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
        TileObjectData.newTile.CoordinatePadding = 0;
        TileObjectData.newTile.Origin = new Point16(2, 4);
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.addTile(Type);
        TileObjectData.addAlternate(Type);

        DustType = DustID.WoodFurniture;
        AddMapEntry(new Color(54, 55, 63), Language.GetOrRegister(Mod.GetLocalizationKey("Tiles.Stylotron.MapEntry")));

        ringTexture = ModContent.Request<Texture2D>(Texture + "Ring");
        ringGlowTexture = ModContent.Request<Texture2D>(Texture + "Ring_Glow");
        flywheelTexture = ModContent.Request<Texture2D>(Texture + "Flywheel");
    }

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = Main.rand.Next(1, 4);
    }

    public override bool CreateDust(int i, int j, ref int type)
    {
        if (Main.rand.NextBool(3))
            type = DustID.Lead;

        return true;
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
    {
        return true;
    }

    public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
    {

    }

    public override bool RightClick(int i, int j)
    {
        return true;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        if (Main.tile[i, j].TileFrameY == 32)
        {
            float mod = MathF.Sin(Main.GlobalTimeWrappedHourly * 0.01f) * 0.2f + 0.7f;
            r = 0.3f * mod;
            g = 0.3f * mod;
            b = 0.9f * mod;
        }
        else
        {
            r = 0f;
            g = 0f;
            b = 0f;
        }
    }

    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
    {
        if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
        {
            Color lightColor = Lighting.GetColor(i, j);
            Color flywheelColor = Lighting.GetColor(i, j);

            Vector2 position = new Vector2((i + 2) * 16 + 1, (j + 2) * 16 - 3);
            if (!Main.drawToScreen)
                position += new Vector2(Main.offScreenRange);

            spriteBatch.Draw(flywheelTexture.Value, position + new Vector2(16, 18) - Main.screenPosition, flywheelTexture.Frame(), flywheelColor, -Main.GlobalTimeWrappedHourly * 7, flywheelTexture.Size() * 0.5f, 1f, 0, 0);

            spriteBatch.Draw(ringTexture.Value, position - Main.screenPosition, ringTexture.Frame(), lightColor, 0, ringTexture.Size() * 0.5f, 1f, 0, 0);
            spriteBatch.Draw(ringGlowTexture.Value, position - Main.screenPosition, ringGlowTexture.Frame(), Color.White with { A = 200 }, 0, ringGlowTexture.Size() * 0.5f, 1f, 0, 0);

        }

        if (Main.gamePaused || !Main.instance.IsActive)
            return;
    }

    private static Asset<Texture2D> ringTexture;
    private static Asset<Texture2D> ringGlowTexture;
    private static Asset<Texture2D> flywheelTexture;
}
