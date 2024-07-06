using BlockVanity.Common.Utilities;
using BlockVanity.Content.Projectiles.Weapons.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Weapons.Magic;

public class ScholarStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.gunProj[Type] = true;
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 34;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.channel = true;
        Item.noUseGraphic = true;
        Item.noMelee = true;

        Item.damage = 60;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 5;
        Item.knockBack = 8f;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.buyPrice(0, 2);
        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.shoot = ModContent.ProjectileType<ScholarStaffProj>();
        Item.shootSpeed = 5f;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile staff = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, -1);
        staff.spriteDirection = -1;
        return false;
    }
}

public class ScholarStaffHeldItemLayer : PlayerDrawLayer
{
    public static SlowAsset<Texture2D> heldTexture;

    public override void Load()
    {
        heldTexture = new SlowAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Items/Weapons/Magic/ScholarStaff_Held");
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => true;//drawInfo.drawPlayer.HeldItem.ModItem is ScholarStaff && drawInfo.drawPlayer.ItemTimeIsZero;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        DrawData staffData = new DrawData(heldTexture.Value, drawInfo.BodyPosition(), heldTexture.Value.Frame(), drawInfo.colorArmorBody, drawInfo.rotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);

    }
}
