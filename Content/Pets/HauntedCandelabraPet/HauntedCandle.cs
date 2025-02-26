using BlockVanity.Content.Items.Placeable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Pets.HauntedCandelabraPet;

public class HauntedCandle : ModItem
{
    public override void SetDefaults()
    {
        Item.buffType = ModContent.BuffType<HauntedCandelabraBuff>();
        Item.DefaultToVanitypet(ModContent.ProjectileType<HauntedCandelabra>(), Item.buffType);
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
            player.AddBuff(Item.buffType, 3600);

        return true;
    }

    //public override void AddRecipes()
    //{
    //    CreateRecipe()
    //        .AddIngredient<HauntedCandleTileItem>()
    //        .AddCondition(Condition.InGraveyard)
    //        .Register();
    //}

    public static Asset<Texture2D> flameTexture;

    public override void Load()
    {
        flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(flameTexture.Value, position, flameTexture.Frame(), Color.White with { A = 200 }, 0, flameTexture.Size() * 0.5f, scale, 0, 0);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        spriteBatch.Draw(flameTexture.Value, Item.Center - Main.screenPosition, flameTexture.Frame(), Color.White with { A = 200 }, rotation, flameTexture.Size() * 0.5f, scale, 0, 0);
    }
}