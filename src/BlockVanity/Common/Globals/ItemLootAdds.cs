using BlockVanity.Content.Items.Vanity.Myrtle;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Globals;

public class ItemLootAdds : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.OceanCrate || item.type == ItemID.OceanCrateHard)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlumeriaHairpin>(), 200));
        }
    }
}