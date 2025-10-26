using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace BlockVanity.Common.Wardrobe;

public sealed class WardrobeUI : UIState
{
    public sealed class WardrobeEntryButton : UIElement
    {
        public WardrobeEntry Entry { get; set; }

        public WardrobeEntryButton(WardrobeEntry entry)
        {
            Entry = entry;
        }
    }

    public WardrobeUI()
    {
        Width.Set(400, 0);
        Height.Set(200, 0);
    }
}
