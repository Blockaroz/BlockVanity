using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Hairs;

public class SpaceWarriorHair : ModHair
{
    public override Gender RandomizedCharacterCreationGender => Gender.Male;

    public override bool AvailableDuringCharacterCreation => true;
}
