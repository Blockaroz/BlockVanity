using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockVanity.Common.Wardrobe;

public enum WardrobeQuestUnlockState
{
    Locked,
    Available,
    Complete,
    Claimed
}

public abstract class WardrobeEntry
{
    public WardrobeQuestUnlockState CompletionState { get; set; }

    public IWardrobePortrait Portrait { get; }

    public int Stars { get; } = 0;

    public virtual bool UnlockCondition() => true;

    public abstract bool CompleteCondition();
}
