using BlockVanity.Content.Projectiles.Weapons.Magic;
using Microsoft.Xna.Framework;
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
