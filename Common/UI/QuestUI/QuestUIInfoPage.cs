using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIInfoPage : UIPanel
{
    private UIList _list;
    private UIScrollbar _scrollbar;
    private bool _isScrollbarAttached;

    public QuestUIInfoPage()
    {
        Width.Set(350f, 0f);
        Height.Set(0f, 1f);
        SetPadding(0f);
        BorderColor = new Color(89, 116, 213);
        BackgroundColor = new Color(73, 94, 171);
        UIList list = _list = new UIList
        {
            Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
            Height = StyleDimension.FromPixelsAndPercent(0f, 1f)
        };

        list.SetPadding(2f);
        list.PaddingBottom = 4f;
        list.PaddingTop = 4f;
        list.ListPadding = 4f;
        list.ManualSortMethod = ManualSort;
        Append(list);

        UIScrollbar scrollBar = _scrollbar = new UIScrollbar();
        scrollBar.SetView(100f, 1000f);
        scrollBar.Height.Set(-20f, 1f);
        scrollBar.HAlign = 1f;
        scrollBar.VAlign = 0.5f;
        scrollBar.Left.Set(-6f, 0f);
        _list.SetScrollbar(_scrollbar);
        _isScrollbarAttached = true;
        CheckScrollBar();

        UIPanel border = new UIPanel
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(0f, 1f),
            IgnoresMouseInteraction = true
        };

        border.BorderColor = new Color(89, 116, 213);
        border.BackgroundColor = Color.Transparent;
        Append(border);
    }

    private void ManualSort(List<UIElement> list)
    {
    }

    public override void Recalculate()
    {
        base.Recalculate();
        CheckScrollBar();
    }

    private void CheckScrollBar()
    {
        if (_scrollbar != null)
        {
            bool canScroll = _scrollbar.CanScroll;
            canScroll = true;
            if (_isScrollbarAttached && !canScroll)
            {
                RemoveChild(_scrollbar);
                _isScrollbarAttached = false;
                _list.Width.Set(0f, 1f);
            }
            else if (!_isScrollbarAttached && canScroll)
            {
                Append(_scrollbar);
                _isScrollbarAttached = true;
                _list.Width.Set(-20f, 1f);
            }
        }
    }
}
