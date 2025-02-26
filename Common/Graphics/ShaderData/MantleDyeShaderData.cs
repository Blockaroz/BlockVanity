using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics.ShaderData;

public class MantleDyeShaderData : ArmorShaderData
{
    public MantleDyeShaderData() : base(AllAssets.Effects.HotMantleDye, "ShaderPass")
    {
    }
}
