using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.Myrtle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BlockVanity.Content.Tiles;

public class PlumeriaPlant : ModTile
{
    public static SlowAsset<Texture2D> glowTexture;

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileObsidianKill[Type] = true;
        Main.tileCut[Type] = true;
        Main.tileNoFail[Type] = true;
        TileID.Sets.ReplaceTileBreakUp[Type] = true;
        TileID.Sets.IgnoredInHouseScore[Type] = true;
        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.CoordinatePadding = 0;
        TileObjectData.newTile.CoordinatePaddingFix = Point16.Zero;

        TileObjectData.addTile(Type);

        HitSound = SoundID.Grass;
        DustType = DustID.SeaOatsBeach;

        RegisterItemDrop(ModContent.ItemType<PlumeriaHairpin>(), 0, 1, 2);

        AddMapEntry(new Color(201, 229, 255), CreateMapEntryName());

        glowTexture = new SlowAsset<Texture2D>(Texture + "_Glow");
    }

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        Vector3 glowColor = Color.SlateGray.ToVector3() * (1f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2f % MathHelper.TwoPi) * 0.1f) * 0.2f;
        r = glowColor.X;
        g = glowColor.Y;
        b = glowColor.Z;
    }

    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
    {
        if (drawData.tileFrameX % 36 == 0 && drawData.tileFrameY % 36 == 0)
            Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        float scale = 1.2f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2f % MathHelper.TwoPi) * 0.2f;
        float rotation = MathF.Sin(Main.GlobalTimeWrappedHourly % MathHelper.TwoPi);
        Color glowColor = Color.SlateGray * (1f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2 % MathHelper.TwoPi + 2f) * 0.9f) * 0.2f;
        glowColor.A = 0;        
        spriteBatch.Draw(glowTexture.Value, new Vector2(i * 16, j * 16) + glowTexture.Value.Size() * 0.5f - Main.screenPosition + new Vector2(!Main.drawToScreen ? Main.offScreenRange : 0), glowTexture.Value.Frame(), (Color.SlateGray * 0.1f) with { A = 0 }, 0f, glowTexture.Value.Size() * new Vector2(0.5f, 0.4f), scale * 0.2f + 0.7f, 0, 0);
        spriteBatch.Draw(glowTexture.Value, new Vector2(i * 16, j * 16) + glowTexture.Value.Size() * 0.5f - Main.screenPosition + new Vector2(!Main.drawToScreen ? Main.offScreenRange : 0), glowTexture.Value.Frame(), glowColor, -rotation * 0.2f, glowTexture.Value.Size() * new Vector2(0.5f, 0.4f), scale * 0.5f + 0.3f, 0, 0);
        spriteBatch.Draw(glowTexture.Value, new Vector2(i * 16, j * 16) + glowTexture.Value.Size() * 0.5f - Main.screenPosition + new Vector2(!Main.drawToScreen ? Main.offScreenRange : 0), glowTexture.Value.Frame(), glowColor * 0.66f, rotation * 0.2f, glowTexture.Value.Size() * new Vector2(0.5f, 0.4f), scale, 0, 0);
    }
}
public class PlumeriaPlantPlacer : ModItem
{
    public override string Texture => $"{nameof(BlockVanity)}/Assets/Textures/Tiles/PlumeriaPlant";

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<PlumeriaPlant>());
    }
}
