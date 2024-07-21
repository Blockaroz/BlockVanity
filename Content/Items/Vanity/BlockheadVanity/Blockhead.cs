using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BlockheadVanity;

[AutoloadEquip(EquipType.Head)]
public class Blockhead : VanityItem
{
    public Blockhead() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override Color? GetAlpha(Color lightColor) => Main.LocalPlayer.skinColor.MultiplyRGBA(lightColor).ToGrayscale();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MiscEffectPlayer>().blockheadSkin = true;

    public override bool IsVanitySet(int head, int body, int legs) => true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 6)
            .AddIngredient(ItemID.Lens, 2)
            .AddIngredient(ItemID.FamiliarWig)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
