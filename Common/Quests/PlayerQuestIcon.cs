using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;

namespace BlockVanity.Common.Quests;

public class PlayerQuestIcon : IQuestEntryIcon
{
    private Player _player;

    public PlayerQuestIcon()
    {
        _player = new Player();
        _player.isDisplayDollOrInanimate = true;
    }

    public void Update(QuestEntry entry, EntryIconDrawSettings settings)
    {
        _player.velocity.X = Utils.ToInt(settings.IsHovered) * 2;

        _player.PlayerFrame();
    }

    public void Draw(QuestEntry entry, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
    {
    }
}
