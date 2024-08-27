using System.Collections.Generic;
using BlockVanity.Common.Quests;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIRewardList : UIElement
{
    private List<int> _rewards;
    private UIScrollbar _scrollbar;
    private bool _isScrollbarAttached;

    private UIList _list;

    public QuestUIRewardList()
    {
        _rewards = new List<int>();

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
