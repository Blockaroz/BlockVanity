using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using BlockVanity.Content.Items.Vanity;
using BlockVanity.Content.Items.Vanity.Scholar;
using BlockVanity.Content.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Quests;

public class ScholarQuest : QuestEntry
{
    public override int StarCount => AllQuests.StarCountRare;

    public override QuestRewardData Reward => new QuestRewardData([
        ModContent.ItemType<ScholarHood>(),
        ModContent.ItemType<ScholarCloak>()], 
        Item.buyPrice(0, 5, 0, 0));

    public override bool IsAvailable() => Main.hardMode;

    public override bool IsComplete() => Main.LocalPlayer.HasItem(ModContent.ItemType<ScholarStaff>());

    public override IQuestPicture GetPortrait()
    {
        Player player = VanityUtils.CreatePlayer(head: ModContent.ItemType<ScholarHood>(), body: ModContent.ItemType<ScholarCloak>());
        player.eyeColor = Color.Red;
        return new PlayerQuestIcon(player);
    }
}
