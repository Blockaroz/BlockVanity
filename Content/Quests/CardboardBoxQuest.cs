using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using BlockVanity.Content.Items.Vanity;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Quests;

public class CardboardBoxQuest : QuestEntry
{
    public override int StarCount => AllQuests.StarCountCommon;

    public override QuestRewardData Reward => new QuestRewardData([
        ModContent.ItemType<CardboardBox>()], 
        Item.buyPrice(0, 2, 50, 0));

    public override bool IsAvailable() => Main.hardMode;

    public override bool IsComplete() => true;

    public override IQuestPicture GetPortrait() => new PlayerQuestIcon(VanityUtils.CreatePlayer(head: ModContent.ItemType<CardboardBox>()));

    public override IQuestPicture GetIcon() => new ItemQuestIcon(ModContent.ItemType<CardboardBox>());
}
