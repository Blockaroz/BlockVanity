using BlockVanity.Common.Utilities;
using BlockVanity.Common.Wardrobe;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.Excellence;
using BlockVanity.Content.Quests.Portraits;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Quests;

public static class QuestDefinitions
{
    static bool AlwaysAvailable(Player player) => true;

    // Name, Stars
    // Portraits
    // 

    public static QuestEntry TheBox()
    {
        var portrait = new WardrobeItemPortrait(ModContent.ItemType<CardboardBox>());
        return new QuestEntry(BlockVanity.Instance, 
            Name: "TheBox",
            Stars: 1,
            SmallPortrait: portrait, 
            LargePortrait: portrait,
            Unlock: AlwaysAvailable,
            Complete: player => player.HasItem(ModContent.ItemType<CardboardBox>()));
    }

    public static QuestEntry Excellence()
    {
        return new QuestEntry(BlockVanity.Instance, 
            Name: "Excellence", 
            Stars: 10,
            SmallPortrait: new WardrobeItemPortrait(ModContent.ItemType<Excellence>()), 
            LargePortrait: new ExcellenceWardrobePortrait(),
            Unlock: AlwaysAvailable,
            Complete: player => player.GetMagicalWardrobe().StarCount >= MagicalWardrobe.TotalStarCount - 10);
    }
}