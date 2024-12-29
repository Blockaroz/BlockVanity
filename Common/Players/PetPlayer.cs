using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
