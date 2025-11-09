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
        public static bool[] ItemDoesNotPayMana = ItemID.Sets.Factory.CreateNamedSet(nameof(BlockVanity), "DoNotPayMana").RegisterBoolSet();
    }

    private void LoadSetBehavior()
    {
        On_Player.ItemCheck_PayMana += CancelManaPayment;
    }

    private bool CancelManaPayment(On_Player.orig_ItemCheck_PayMana orig, Player self, Item sItem, bool canUse)
    {
        if (Sets.ItemDoesNotPayMana[sItem.type])
            return self.CheckMana(sItem.mana, false);

        return orig(self, sItem, canUse);
    }
}
