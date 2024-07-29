using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.GameContent.Bestiary;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIEntryGridButton : UIElement
{
    public static readonly int EntryWidth = 110;
    public static readonly int EntryHeight = 132;

    private QuestUIEntryIcon _icon;

    public QuestEntry Entry { get; private set; }

    //remove
    private string _hoverName;

    public QuestUIEntryGridButton(QuestEntry entry)
    {
        Entry = entry;
        Height.Set(EntryWidth, 0f);
        Width.Set(EntryHeight, 0f);

        UIElement mainElement = new UIElement
        {
            Width = new StyleDimension(-4f, 1f),
            Height = new StyleDimension(-4f, 1f),
            IgnoresMouseInteraction = true,
            OverflowHidden = true,
            HAlign = 0.5f,
            VAlign = 0.5f
        };

        mainElement.SetPadding(0f);
        QuestUIEntryIcon icon = _icon = new QuestUIEntryIcon(entry, false);
        mainElement.Append(icon);
        Append(mainElement);

        base.OnMouseOver += MouseOver;
        base.OnMouseOut += MouseOut;
    }

    private void MouseOut(UIMouseEvent evt, UIElement listeningElement)
    {
        _icon.ForceHover = false;
    }

    private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
    {
        SoundEngine.PlaySound(SoundID.MenuTick);
        _icon.ForceHover = true;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (IsMouseHovering)
            Main.instance.MouseText(Entry.Name.Value, 0, 0);
    }
}
