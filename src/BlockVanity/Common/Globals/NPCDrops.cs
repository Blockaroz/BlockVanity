using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Common.Globals;

public class NPCDrops : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        //if (npc.type == NPCID.Demon)
        //    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<DemonHead>(), 100));
    }
}