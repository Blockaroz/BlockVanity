using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;

namespace BlockVanity.Content.Vanity.Myrtle;

public class PlumeriaPlantItem : ModItem
{
    public override string Texture => Assets.Textures.Content.Vanity.Myrtle.PlumeriaPlant_Item.KEY;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<PlumeriaPlantTile>());
    }

    public static void MakePlumeriaRenewable(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.OceanCrate || item.type == ItemID.OceanCrateHard)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlumeriaHairpinItem>(), 200));
        }
    }
}

public sealed class PlumeriaPlantTile : ModTile
{
    public override string Texture => Assets.Textures.Content.Vanity.Myrtle.PlumeriaPlant_Tile.KEY;

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
        TileObjectData.newTile.Width = 1;
        TileObjectData.newTile.Height = 1;
        TileObjectData.newTile.CoordinateWidth = 28;
        TileObjectData.newTile.CoordinateHeights = [16];
        TileObjectData.newTile.DrawYOffset = 2;

        TileObjectData.addTile(Type);

        HitSound = SoundID.Grass;
        DustType = DustID.SeaOatsBeach;

        RegisterItemDrop(ModContent.ItemType<PlumeriaHairpinItem>(), 0, 1, 2);

        AddMapEntry(new Color(201, 229, 255), CreateMapEntryName());
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
        Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        var glowTexture = Assets.Textures.Content.Vanity.Myrtle.PlumeriaPlant_Tile_Glow.Asset;
        float scale = 1.2f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2f % MathHelper.TwoPi) * 0.2f;
        float rotation = MathF.Sin(Main.GlobalTimeWrappedHourly % MathHelper.TwoPi);
        Color glowColor = Color.SlateGray * (1f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2 % MathHelper.TwoPi + 2f) * 0.9f) * 0.2f;
        glowColor.A = 0;

        Vector2 position = new Vector2(i * 16 - 6, j * 16) + glowTexture.Size() * 0.5f - Main.screenPosition + new Vector2(Main.offScreenRange);
        Vector2 origin = glowTexture.Size() * new Vector2(0.5f, 0.4f);
        spriteBatch.Draw(glowTexture.Value, position, null, (Color.SlateGray * 0.1f) with { A = 0 }, 0f, origin, scale * 0.2f + 0.7f, 0, 0);
        spriteBatch.Draw(glowTexture.Value, position, null, glowColor, -rotation * 0.2f, origin, scale * 0.5f + 0.3f, 0, 0);
        spriteBatch.Draw(glowTexture.Value, position, null, glowColor * 0.66f, rotation * 0.2f, origin, scale, 0, 0);
    }
}