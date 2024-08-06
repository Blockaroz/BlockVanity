using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIEntryGridButton : UIElement
{
    public static readonly int EntryWidth = 110;
    public static readonly int EntryHeight = 132;

    private QuestUIEntryIcon _icon;

    public QuestEntry Entry { get; private set; }

    public QuestUIEntryGridButton(QuestEntry entry)
    {
        Entry = entry;
        Width.Set(EntryWidth, 0f);
        Height.Set(EntryHeight, 0f);
        SetPadding(0f);

        UIPanel mainElement = new UIPanel
        {
            Width = new StyleDimension(-4f, 1f),
            Height = new StyleDimension(-4f, 1f),
            IgnoresMouseInteraction = true,
            OverflowHidden = true,
            HAlign = 0.5f,
            VAlign = 0.5f,
            BackgroundColor = Color.Black * 0.2f,
            BorderColor = Color.Transparent
        };
        mainElement.SetPadding(0f);

        QuestUIEntryIcon icon = _icon = new QuestUIEntryIcon(entry, false);
        mainElement.Append(icon);

        Append(mainElement);

        OnMouseOver += MouseOver;
        OnMouseOut += MouseOut;
    }

    private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
    {
        SoundEngine.PlaySound(SoundID.MenuTick);
        _icon.ForceHover = true;
    }

    private void MouseOut(UIMouseEvent evt, UIElement listeningElement)
    {
        _icon.ForceHover = false;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (IsMouseHovering)
        {
            string text = "???";
            if (Entry.Completion != QuestCompletionState.Hidden)
                text = Entry.Name.Value;

            Main.instance.MouseText(text, 0, 0);
        }
    }
}
