using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.CountChaos;

[AutoloadEquip(EquipType.Legs)]
public class CountChaosGown : VanityItem, IUpdateArmorInVanity
{
    public CountChaosGown() : base(ItemRarityID.Cyan) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;

        MiscEffectPlayer.hideLegs.Add(Item.legSlot);
    }

    public override void UpdateEquip(Player player) => player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
}
