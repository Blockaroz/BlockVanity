using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUIPortrait : UIElement
{
    private IQuestPicture _portrait;

    public QuestUIPortrait()
    {

    }

    public void SetEntry(QuestEntry entry)
    {
        if (entry == null)
        {

        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        
    }
}
