using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class PetPlayer : ModPlayer
{
    public bool hauntedCandelabra;

    public override void ResetEffects()
    {
        hauntedCandelabra = false;
    }
}
