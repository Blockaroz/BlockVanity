using BlockVanity.Content.Projectiles.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Weapons.Magic;

public class ScholarStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.gunProj[Type] = true;
        ItemID.Sets.CanBePlacedOnWeaponRacks[Type] = true;
        BlockVanity.Sets.ItemDoesNotPayMana[Type] = true;
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
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.shoot = ModContent.ProjectileType<ScholarStaffProj>();
        Item.shootSpeed = 8f;
        Item.autoReuse = true;
    }

    public override Color? GetAlpha(Color lightColor) => Color.Lerp(Color.White, lightColor * 1.5f, 0.9f) with { A = 180 };
}