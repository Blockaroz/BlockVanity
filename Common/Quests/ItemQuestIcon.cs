using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace BlockVanity.Common.Quests;

public class ItemQuestIcon : IQuestEntryIcon
{
    private Item _item;
    private float _fade;

    public ItemQuestIcon(int itemType)
    {
        _item = new Item(itemType);
    }

    public void Update(QuestEntry entry, EntryIconDrawSettings settings)
    {
        if (settings.IsPortrait)
            _fade = 0f;
        else
        {
            _fade = MathHelper.Lerp(_fade, Utils.ToInt(settings.IsHovered), settings.IsHovered ? 0.2f : 0.1f);
            if (_fade < 0.1f)
                _fade = 0f;
            if (_fade > 0.99f)
                _fade = 1f;
        }
    }

    public void Draw(QuestEntry entry, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
    {
        Color drawColor = Color.White;
        float drawScale = 1.5f + _fade * 0.5f;

        if (entry.Completion == QuestCompletionState.Hidden)
            drawColor = Color.Black;

        Texture2D shadow = AllAssets.Textures.Glow[0].Value;
        spriteBatch.Draw(shadow, settings.iconbox.Center(), shadow.Frame(), Color.Black * 0.2f, 0, shadow.Size() * 0.5f, 1f, 0, 0);

        ItemSlot.DrawItemIcon(_item, 31, spriteBatch, settings.iconbox.Center(), drawScale, 2048, drawColor);
    }
}
