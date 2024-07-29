using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BlockVanity.Common.Quests;

public static class QuestFilters
{
    public class BySearch : IEntryFilter<QuestEntry>, ISearchFilter<QuestEntry>
    {
        private string _search;

        public bool FitsFilter(QuestEntry entry)
        {
            if (_search == null)
                return true;

            return true;
        }

        public string GetDisplayNameKey() => "BestiaryInfo.IfSearched";

        public UIElement GetImage()
        {
            Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light");
            return new UIImageFramed(asset, asset.Frame())
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
        }

        public void SetSearch(string searchText)
        {
            _search = searchText;
        }
    }

    public class ByCompletionState : IEntryFilter<QuestEntry>
    {
        public bool FitsFilter(QuestEntry entry)
        {
            return true;
        }

        public string GetDisplayNameKey() => "BestiaryInfo.IfUnlocked";

        public UIElement GetImage()
        {
            Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow");
            return new UIImageFramed(asset, asset.Frame(16, 5, 14, 3))
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
        }
    }
}
