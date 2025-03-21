﻿using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Hairs;

public class FishyHair : ModHair
{
    public override Gender RandomizedCharacterCreationGender => Gender.Female;

    public override bool AvailableDuringCharacterCreation => true;

    public override void SetStaticDefaults()
    {
        HairID.Sets.DrawBackHair[Type] = true;
    }
}