using BlockVanity.Common;
using BlockVanity.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Misc;

public class BlackEyeLensItem() : VanityItem(accessory: true)
{
    public override string Texture => Assets.Textures.Content.Vanity.Misc.BlackEyeLens_Item.KEY;

    public override int Rarity => ItemRarityID.Blue;

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<BlackEyeLensPlayer>().Enabled = !hideVisual;

    public override void UpdateVanity(Player player) => UpdateAccessory(player, false);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.BlackLens, 2)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}

public sealed class BlackEyeLensPlayer : ModPlayer
{
    public bool Enabled { get; set; }

    public override void ResetEffects()
    {
        Enabled = false;
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (Enabled)
            drawInfo.colorEyeWhites = new Color(20, 20, 20);
    }
}