using BlockVanity.Content.Projectiles.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Tools;

public class ObliterationRay : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.IsDrill[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 70;
        Item.DamageType = DamageClass.Melee; // Scale with melee speed

        Item.width = 20;
        Item.height = 20;

        Item.useTime = 5;
        Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.channel = true;

        Item.rare = ItemRarityID.LightRed;
        Item.axe = 195 / 5;
        Item.pick = 110;
        Item.tileBoost = 6;

        Item.shoot = ModContent.ProjectileType<ObliterationRayHeld>();
        Item.shootSpeed = 5;
        Item.value = 308;
    }

    public static Asset<Texture2D> glowTexture;

    public override void Load()
    {
        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void HoldItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer && !Main.SmartCursorWanted)
            VanityUtils.ForceSmartCursor(player, true);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), Color.PaleGoldenrod with { A = 0 }, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, glowTexture.Frame(), Color.PaleGoldenrod with { A = 0 }, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);
    }

}
