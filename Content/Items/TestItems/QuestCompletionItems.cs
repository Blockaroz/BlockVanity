using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.TestItems;

public class QuestResetter : ModItem
{
    public override string Texture => AllAssets.Textures.Placeholder;

    public override Color? GetAlpha(Color lightColor) => new Color(255, 40, 40);

    public override void SetDefaults()
    {
        Item.UseSound = SoundID.Item119;
        Item.useTurn = true;
        Item.useAnimation = Item.useTime = 17;
        Item.useStyle = ItemUseStyleID.RaiseLamp;
        Item.consumable = true;
        Item.width = 24;
        Item.height = 24;
        Item.rare = ModContent.RarityType<VanityRareCommon>();
    }

    public override bool? UseItem(Player player)
    {
        foreach (QuestEntry item in QuestSystem.database.Entries)
        {
            item.Completion = QuestCompletionState.Hidden;
        }

        return true;
    }
}

public class QuestCompleter : ModItem
{
    public override string Texture => AllAssets.Textures.Placeholder;

    public override Color? GetAlpha(Color lightColor) => Color.White;

    public override void SetDefaults()
    {
        Item.UseSound = SoundID.Item119;
        Item.useTurn = true;
        Item.useAnimation = Item.useTime = 17;
        Item.useStyle = ItemUseStyleID.RaiseLamp;
        Item.consumable = true;
        Item.width = 24;
        Item.height = 24;
        Item.rare = ModContent.RarityType<VanityRareCommon>();
    }

    public override bool? UseItem(Player player)
    {
        foreach (QuestEntry item in QuestSystem.database.Entries)
        {
            if (item.Completion != QuestCompletionState.Claimed)
                item.Completion = QuestCompletionState.Completed;
        }

        return true;
    }
}
