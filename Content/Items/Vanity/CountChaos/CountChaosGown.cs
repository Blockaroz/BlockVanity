using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.DataStructures;
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
    }

    public override void UpdateEquip(Player player) => player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegs;
    }

    private void HideLegs(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.legs != EquipLoader.GetEquipSlot(Mod, nameof(CountChaosGown), EquipType.Legs))
            orig(ref drawinfo);
    }
}
