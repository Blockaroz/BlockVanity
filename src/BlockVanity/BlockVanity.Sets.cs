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
        public static bool[] ItemDoesNotPayMana;

        public static bool[] HideHead;
        public static bool[] HideLegs;
    }

    private sealed class SetInitializer : ModSystem
    {
        public override void ResizeArrays()
        {
            Sets.ItemDoesNotPayMana = ItemID.Sets.Factory.CreateNamedSet(nameof(BlockVanity), "DoNotPayMana").RegisterBoolSet();
            On_Player.ItemCheck_PayMana += CancelManaPayment;

            Sets.HideHead = ArmorIDs.Head.Sets.Factory.CreateNamedSet(nameof(BlockVanity), "HideHead").RegisterBoolSet();
            Sets.HideLegs = ArmorIDs.Legs.Sets.Factory.CreateNamedSet(nameof(BlockVanity), "HideLegs").RegisterBoolSet();
        }

        private bool CancelManaPayment(On_Player.orig_ItemCheck_PayMana orig, Player self, Item sItem, bool canUse)
        {
            if (Sets.ItemDoesNotPayMana[sItem.type])
                return self.CheckMana(sItem.mana, false);

            return orig(self, sItem, canUse);
        }
    }
}
