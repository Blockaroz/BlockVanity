using BlockVanity.Content.Items.Vanity;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Globals;

public class NPCDrops : GlobalNPC
{
    // There are not enough drops to warrant separate classes.

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.Demon)
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<DemonHead>(), 100));
    }
}