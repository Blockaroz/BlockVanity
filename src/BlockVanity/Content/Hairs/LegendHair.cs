using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Hairs;

public sealed class LegendHair : ModHair
{
    public override Gender RandomizedCharacterCreationGender => Gender.Male;

    public override IEnumerable<Condition> GetUnlockConditions() => [Condition.Hardmode];
}
