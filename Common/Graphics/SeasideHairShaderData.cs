using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics;

public class SeasideHairShaderData : HairShaderData
{
    public SeasideHairShaderData() : base(AllAssets.Effects.SeasideHairShader, "ShaderPass") { }

    public override void Apply(Player player, DrawData? drawData = null)
    {
        base.Apply(player, drawData);
        UseImage(AllAssets.Textures.SeasideColorMap.asset);

        Apply();
    }
}
