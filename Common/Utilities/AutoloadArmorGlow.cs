using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BlockVanity.Common.Utilities;

[AttributeUsage(AttributeTargets.Class)]
public class AutoloadArmorGlow : Attribute
{
    public readonly EquipType[] equipTypes;

    public AutoloadArmorGlow(params EquipType[] equipType)
    {
        equipTypes = equipType;
    }
}
