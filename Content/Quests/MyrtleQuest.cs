using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using BlockVanity.Content.Hairs;
using BlockVanity.Content.Items.Dyes;
using BlockVanity.Content.Items.Vanity.Myrtle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Quests;

public class MyrtleQuest : QuestEntry
{
    public override int StarCount => AllQuests.StarCountUncommon;

    public override QuestRewardData Reward => new QuestRewardData([
        ModContent.ItemType<MyrtleDress>(),
        ModContent.ItemType<MyrtleSandals>(),
        ModContent.ItemType<FishFood>(),
        ModContent.ItemType<SeasideHairDye>()], 
        Item.buyPrice(0, 3, 50, 0));

    public override bool IsAvailable() => true;

    public override bool IsComplete() => Main.LocalPlayer.HasItem(ModContent.ItemType<PlumeriaHairpin>());

    public override IQuestPicture GetPortrait()
    {
        Player player = VanityUtils.CreatePlayer(head: ModContent.ItemType<PlumeriaHairpin>(), body: ModContent.ItemType<MyrtleDress>(), legs: ModContent.ItemType<MyrtleSandals>(), acc1: ModContent.ItemType<FishFood>());
        player.hair = ModContent.GetInstance<FishyHair>().Type;
        player.hairDye = ContentSamples.ItemsByType[ModContent.ItemType<SeasideHairDye>()].hairDye;
        return new PlayerQuestIcon(player);
    }
}
