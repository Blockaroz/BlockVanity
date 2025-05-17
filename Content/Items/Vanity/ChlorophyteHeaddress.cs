using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity;

[AutoloadEquip(EquipType.Head)]
public class ChlorophyteHeaddress : VanityItem
{
    public ChlorophyteHeaddress() : base(ItemRarityID.White, 30, 28, Item.buyPrice(0, 0, 0, 5)) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 8)
            .AddIngredient(ItemID.Feather, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}

public class ChlorophyteHeaddressFeathers : PlayerDrawLayer
{
    public static Asset<Texture2D> FeathersTexture { get; set; }

    public override void Load()
    {
        FeathersTexture = ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/ChlorophyteHeaddress_HeadFeathers");
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeadBack);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, nameof(ChlorophyteHeaddress), EquipType.Head);
    
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Vector2 headPosition = drawInfo.HeadPosition() + new Vector2(-8 * drawInfo.drawPlayer.direction, -8 * drawInfo.drawPlayer.gravDir);
        headPosition.ApplyVerticalOffset(drawInfo);
        Vector2 origin = new Vector2(FeathersTexture.Width() / 2, drawInfo.headVect.Y);

        float rotation = drawInfo.drawPlayer.headRotation 
            - MathHelper.Clamp(drawInfo.drawPlayer.velocity.X * 0.02f, -0.2f, 0.2f) * drawInfo.drawPlayer.gravDir
            + MathHelper.Clamp(drawInfo.drawPlayer.velocity.Y * 0.01f * drawInfo.drawPlayer.gravDir, -0.2f, 0f) * drawInfo.drawPlayer.direction * drawInfo.drawPlayer.gravDir;

        DrawData data = new DrawData(FeathersTexture.Value, headPosition, FeathersTexture.Frame(), drawInfo.colorArmorHead * 1.5f, rotation, origin, 1f, drawInfo.playerEffect);
        data.shader = drawInfo.cHead;
        drawInfo.DrawDataCache.Add(data);
    }
}