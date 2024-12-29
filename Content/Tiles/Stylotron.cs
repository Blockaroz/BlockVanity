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
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BlockVanity.Content.Tiles;

public class Stylotron : ModTile
{
    public static Asset<Texture2D> ringTexture;
    public static Asset<Texture2D> ringGlowTexture;
    public static Asset<Texture2D> flywheelTexture;

    public override void SetStaticDefaults()
    {   
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileNoAttach[Type] = true;

        TileID.Sets.HasOutlines[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
        TileObjectData.newTile.Height = 4;
        TileObjectData.newTile.Height = 5;
        TileObjectData.newTile.CoordinatePadding = 0;
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
        TileObjectData.newTile.Origin = new Point16(2, 4);
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.Direction = TileObjectDirection.None;
        TileObjectData.addTile(Type);

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
        if (Main.rand.NextBool())
            type = DustID.Lead;

        return true;
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

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

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Main.instance.TilesRenderer.AddSpecialPoint(i, j, Terraria.GameContent.Drawing.TileDrawing.TileCounterType.Tree);

        return true;
    }
}
