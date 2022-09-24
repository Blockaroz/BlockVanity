using BlockVanity.Content.Items.TrailItems;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Globals
{
    public class NPCLoot : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, Terraria.ModLoader.NPCLoot npcLoot)
        {
            if (npc.type == NPCID.RainbowSlime)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrismaticRockCrystal>(), 6));

        }
    }
}
