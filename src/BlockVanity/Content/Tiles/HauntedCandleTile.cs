using BlockVanity.Content.Dusts;
using BlockVanity.Content.Pets.HauntedCandelabraPet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BlockVanity.Content.Tiles;

public class HauntedCandleTile : ModTile
{
    public static Asset<Texture2D> flameTexture;

    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileWaterDeath[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.newTile.DrawFlipHorizontal = true;
        TileObjectData.newTile.StyleLineSkip = 2;
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.WaterDeath = true;
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(253, 221, 3), Language.GetText("MapObject.Candle"));
        flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        Item candle = new Item(ModContent.ItemType<HauntedCandle>());
        yield return candle;
    }

    public override bool RightClick(int i, int j)
    {
        HitWire(i, j);
        return true;
    }

    public override void HitWire(int i, int j)
    {
        Tile tile = Main.tile[i, j];
        int topY = j - tile.TileFrameY / 18 % 2;
        short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);

        Main.tile[i, topY].TileFrameX += frameAdjustment;
        Main.tile[i, topY + 1].TileFrameX += frameAdjustment;

        Wiring.SkipWire(i, topY);
        Wiring.SkipWire(i, topY + 1);
        Wiring.SkipWire(i, topY + 2);

        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
        }
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        Tile tile = Main.tile[i, j];
        if (tile.TileFrameX == 0)
        {
            r = 0.1f;
            g = 0.4f;
            b = 0.6f;
        }
    }
    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
    {
        if (Main.gamePaused || !Main.instance.IsActive || Lighting.UpdateEveryFrame && !Main.rand.NextBool(4))
            return;

        Tile tile = Main.tile[i, j];

        if (!TileDrawing.IsVisible(tile))
            return;

        short frameX = tile.TileFrameX;
        short frameY = tile.TileFrameY;

        if (frameX != 0)
            return;

        if (frameY / 18 % 2 == 0)
        {
            Dust flame = Dust.NewDustPerfect(new Vector2(i * 16 + 8, j * 16 + 4) + Main.rand.NextVector2Circular(2, 4), ModContent.DustType<HauntedFlameDust>(), Main.rand.NextVector2Circular(1, 1) - Vector2.UnitY, 0, Color.White with { A = 30 }, Main.rand.NextFloat(0.1f, 0.5f));
            flame.noGravity = !Main.rand.NextBool(5);
        }
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile tile = Main.tile[i, j];

        if (!TileDrawing.IsVisible(tile))
            return;

        Vector2 offScreen = new Vector2(Main.drawToScreen ? 0 : Main.offScreenRange);

        int width = 16;
        int offsetY = 0;
        int height = 16;
        short frameX = tile.TileFrameX;
        short frameY = tile.TileFrameY;

        TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);

        ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i);

        for (int c = 0; c < 5; c++)
        {
            float shakeX = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
            float shakeY = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

            spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + offScreen, new Rectangle(frameX, frameY, width, height), new Color(100, 100, 100, 0), 0f, Vector2.Zero, 1f, 0, 0f);
        }
    }
}