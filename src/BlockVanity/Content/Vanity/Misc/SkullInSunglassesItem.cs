using BlockVanity.Common;
using BlockVanity.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Misc;

[AutoloadEquip(EquipType.Head)]
public class SkullInSunglassesItem : VanityItem, IUpdateArmorInVanity
{
    public override string Texture => Assets.Textures.Content.Vanity.Misc.SkullInSunglasses_Item.KEY;

    public override int Rarity => ItemRarityID.Blue;

    public override void OnCreated(ItemCreationContext context)
    {
        SoundStyle createSound = SoundID.NPCHit2;
        createSound.PitchVariance = 0;
        createSound.MaxInstances = 0;
        createSound.Type = SoundType.Ambient;
        if (!Main.dedServ && !Main.gameMenu)
        {
            SoundEngine.PlaySound(createSound, Main.LocalPlayer.position);
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Skull)
            .AddIngredient(ItemID.Sunglasses)
            .AddCondition(Condition.InGraveyard)
            .Register();
    }

    public override void UpdateEquip(Player player) => HitEffectPlayer.SetEquipHitSound(player, SoundID.NPCHit2 with { PitchVariance = 0.5f });
}