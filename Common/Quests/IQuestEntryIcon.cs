using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;

namespace BlockVanity.Common.Quests;

public interface IQuestEntryIcon
{
    public void Update(QuestEntry entry, EntryIconDrawSettings settings);

    public void Draw(QuestEntry entry, SpriteBatch spriteBatch, EntryIconDrawSettings settings);
}
