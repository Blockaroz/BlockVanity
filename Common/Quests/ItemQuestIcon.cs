using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace BlockVanity.Common.Quests;

public class ItemQuestIcon : IQuestEntryIcon
{
    private Item _item;

    public ItemQuestIcon(int itemType)
    {
        _item = new Item(itemType);
    }

    public void Update(QuestEntry entry, EntryIconDrawSettings settings)
    {
    }

    public void Draw(QuestEntry entry, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
    {
        // UIItemIcon uses context 31, which has extra logic 
        Color drawColor = Color.White;
        float drawScale = settings.IsPortrait ? 2f : 1.5f;

        if (entry.Completion == QuestCompletionState.Hidden)
            drawColor = Color.Black;

        ItemSlot.DrawItemIcon(_item, 31, spriteBatch, settings.iconbox.Center(), drawScale, 2048, drawColor);
    }
}
