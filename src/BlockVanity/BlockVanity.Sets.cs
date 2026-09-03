using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity;

public partial class BlockVanity
{
    public static class Sets
    {
        public static bool[] ItemDoesNotPayMana { get; set; } = [];

        public static bool[] HideLegs { get; set; } = [];
    }

    private sealed class SetInitializer : ModSystem
    {
        public override void ResizeArrays()
        {
            Sets.ItemDoesNotPayMana = ItemID.Sets.Factory.CreateNamedSet(nameof(BlockVanity), "DoNotPayMana").RegisterBoolSet();
            On_Player.ItemCheck_PayMana_ShouldSkipManaUse += CancelManaPayment; ;

            Sets.HideLegs = ArmorIDs.Legs.Sets.Factory.CreateNamedSet(nameof(BlockVanity), "HideLegs").RegisterBoolSet();
        }

        private bool CancelManaPayment(On_Player.orig_ItemCheck_PayMana_ShouldSkipManaUse orig, Player self, Item sItem, bool altFire)
        {
            if (Sets.ItemDoesNotPayMana[sItem.type])
                return true;

            return orig(self, sItem, altFire);
        }
    }
}
