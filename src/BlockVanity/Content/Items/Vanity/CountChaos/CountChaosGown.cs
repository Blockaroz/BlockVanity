using BlockVanity.Common.Players;
using BlockVanity.Content.Rarities;
using BlockVanity.Content.Rarities.GlowingRarities;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.CountChaos;

[AutoloadEquip(EquipType.Legs)]
public class CountChaosGown : VanityItem, IUpdateArmorInVanity
{
    public override int Rarity => ModContent.RarityType<RarityCommonVanity>();

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;

        BlockVanity.Sets.HideLegs[Item.legSlot] = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(0.275f);
    }
}