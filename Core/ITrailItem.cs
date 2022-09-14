using System;
using Terraria;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Core
{
    internal interface ITrailItem
    {
        public abstract void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye);
    }
}
