using BlockVanity.Content.Items.Pets.ImpishEye;
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

            if (npc.type == NPCID.DemonEye || npc.type == NPCID.DemonEye2)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LooseEye>(), 50));
        }
    }
}
