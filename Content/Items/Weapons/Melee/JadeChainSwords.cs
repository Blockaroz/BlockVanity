using BlockVanity.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Weapons.Melee;

public class JadeChainSwords : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.CanBePlacedOnWeaponRacks[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 38;
        Item.height = 42;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.channel = true;
        Item.noUseGraphic = true;
        Item.noMelee = true;

        Item.damage = 80;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 2f;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(0, 5);
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.shoot = ModContent.ProjectileType<JadeChainSwordProj>();
        Item.shootSpeed = 8f;
        Item.autoReuse = true;;
    }

    public override Color? GetAlpha(Color lightColor) => lightColor * 1.5f;

    public override void Update(ref float gravity, ref float maxFallSpeed) => Lighting.AddLight(Item.Center, Color.Teal.ToVector3());

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 2;

    private int combo;
    private int direction;

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            if (direction == 0)
                direction = 1;

            switch (combo)
            {
                default:
                case 0:
                case 1:

                    float rotation = Main.rand.NextFloat(0.6f) * direction;
                    Projectile newProj = Projectile.NewProjectileDirect(source, position - velocity.SafeNormalize(Vector2.Zero) * 5, velocity.RotatedBy(rotation), type, damage, knockback, player.whoAmI);
                    newProj.ai[1] = 0;
                    newProj.ai[2] = Math.Abs(rotation);
                    newProj.spriteDirection = direction;

                    break;

                case 2:
                case 3:

                    newProj = Projectile.NewProjectileDirect(source, position - velocity.SafeNormalize(Vector2.Zero) * 5, velocity.RotatedByRandom(0.2f), type, damage, knockback, player.whoAmI);
                    newProj.ai[1] = 1;
                    newProj.ai[2] = Main.rand.NextFloat(1f, 1.5f);
                    newProj.direction = direction;

                    break;
            }
            direction = Main.rand.NextBool() ? 1 : -1;

            combo++;
            if (combo > 3)
                combo = 0;
        }

        return false;
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        //int swordsLeft = 2 - Main.LocalPlayer.ownedProjectileCounts[Item.shoot];
        //Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, swordsLeft.ToString(), position.X + 8, position.Y + 4, Color.White, Color.DarkGreen, new Vector2(0.5f));
    }
}