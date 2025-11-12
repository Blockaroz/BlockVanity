using BlockVanity.Common.Utilities;
using BlockVanity.Common.Wardrobe;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.Excellence;
using BlockVanity.Content.Items.Weapons.Magic;
using BlockVanity.Content.NPCs.Misc;
using BlockVanity.Content.Quests.Portraits;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Quests;

public static class QuestDefinitions
{
    static bool AlwaysAvailable(Player player) => true;
    static bool Hardmode(Player player) => true;

    public static class Common
    {
        public static QuestEntry TheBox()
        {
            var portrait = new WardrobeItemPortrait(ModContent.ItemType<CardboardBox>());
            return new QuestEntry(BlockVanity.Instance,
                Name: "TheBox",
                Stars: 1,
                SmallPortrait: portrait,
                LargePortrait: portrait,
                Unlock: AlwaysAvailable,
                Complete: player => player.HasItem(ModContent.ItemType<CardboardBox>()),
                Reward: QuestReward.None);
        }
    }

    public static class Tough
    {
        public static QuestEntry ScholarOfOld()
        {
            return new QuestEntry(BlockVanity.Instance,
                Name: "ScholarOfOld",
                Stars: 3,
                SmallPortrait: new WardrobeNPCPortrait(ModContent.NPCType<ScholarStaffGhost>()),
                LargePortrait: new WardrobePlayerPortrait(new Player()),
                Unlock: Hardmode,
                Complete: player => player.HasItem(ModContent.ItemType<ScholarStaff>()),
                Reward: QuestReward.None);
        }
    }

    public static class Special
    {
        public static QuestEntry Excellence()
        {
            return new QuestEntry(BlockVanity.Instance,
                Name: "Excellence",
                Stars: 10,
                SmallPortrait: new WardrobeItemPortrait(ModContent.ItemType<Excellence>()),
                LargePortrait: new ExcellenceWardrobePortrait(),
                Unlock: AlwaysAvailable,
                Complete: player => player.GetMagicalWardrobe().StarCount >= MagicalWardrobe.TotalStarCount - 10,
                Reward: QuestReward.None);
        }
    }
}