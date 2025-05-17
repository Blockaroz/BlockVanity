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

public class WitheringDyeShaderData : ArmorShaderData
{
    public WitheringDyeShaderData() : base(AllAssets.Effects.WitheringDye, "ShaderPass")
    {
    }
}
