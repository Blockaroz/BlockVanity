using BlockVanity.Common.Utilities;
using BlockVanity.Common.Wardrobe;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.CountChaos;
using BlockVanity.Content.Items.Vanity.Excellence;
using BlockVanity.Content.Items.Weapons.Magic;
using BlockVanity.Content.NPCs.Misc;
using BlockVanity.Content.Quests.Portraits;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Quests;

public static class QuestDefinitions
{
    public static class Common
    {
        public static QuestEntry TheBox()
        {
            var portrait = new WardrobeItemIcon(ModContent.ItemType<CardboardBox>());
            return new QuestEntry(BlockVanity.Instance,
                Name: "TheBox",
                Stars: 1,
                Icon: portrait,
                Portrait: portrait,
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
                Icon: new WardrobeItemIcon(ModContent.ItemType<ScholarStaff>()),
                Portrait: new WardrobePlayerPortrait(new Player()),
                Reward: QuestReward.None);
        }
    }

    public static class Special
    {
        public static QuestEntry ChaosGarden()
        {
            Player player = new Player();
            player.armor[0] = new Item(ModContent.ItemType<CountChaosHornedHead>());
            player.armor[0] = new Item(ModContent.ItemType<CountChaosHornedHead>());

            var portrait = new WardrobePlayerPortrait(player);
            return new QuestEntry(BlockVanity.Instance,
                Name: "ChaosGarden",
                Stars: 7,
                Icon: portrait,
                Portrait: portrait,
                Reward: QuestReward.None);
        }

        public static QuestEntry Excellence()
        {
            return new QuestEntry(BlockVanity.Instance,
                Name: "Excellence",
                Stars: 10,
                Icon: new ExcellenceWardrobeIcon(),
                Portrait: new ExcellenceWardrobePortrait(),
                Reward: QuestReward.None);
        }
    }
}