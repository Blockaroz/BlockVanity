using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Hairs;

public class FishyHair : ModHair
{
    public override string Texture => Assets.Textures.Content.Hairs.Fishy_Hair.KEY;

    public override Gender RandomizedCharacterCreationGender => Gender.Female;

    public override bool AvailableDuringCharacterCreation => true;

    public override void SetStaticDefaults()
    {
        HairID.Sets.DrawBackHair[Type] = true;
    }
}