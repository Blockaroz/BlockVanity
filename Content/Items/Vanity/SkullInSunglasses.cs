using BlockVanity.Core;
using Terraria.ModLoader;
using System;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.Audio;
using BlockVanity.Common.Players;

namespace BlockVanity.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class SkullInSunglasses : VanityItem
    {
        public SkullInSunglasses() : base("Skull in Sunglasses", ItemRarityID.Blue, "'Bad!'") { }

        public override void OnCreate(ItemCreationContext context)
        {
            if (!Main.gameMenu)
                SoundEngine.PlaySound(SoundID.NPCHit2, Main.LocalPlayer.Center);
        }

        public override void PreUpdateVanitySet(Player player)
        {
            player.GetModPlayer<MiscEffectPlayer>().boneSound = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Skull)
                .AddIngredient(ItemID.Sunglasses)
                .AddCondition(Recipe.Condition.InGraveyardBiome)
                .Register();
        }
    }
}
