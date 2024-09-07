using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics;

public class PhantomDyeShaderData : ArmorShaderData
{
    public PhantomDyeShaderData() : base(AllAssets.Effects.PhantomDye, "ShaderPass") { }

    public override void Apply(Entity entity, DrawData? drawData = null)
    {
        UseImage(Main.Assets.Request<Texture2D>("Images/Misc/noise"));
        base.Apply(entity, drawData);
        Shader.Parameters["uTime"]?.SetValue(Main.GlobalTimeWrappedHourly / 10f);
        Shader.CurrentTechnique.Passes[0].Apply();
    }
}
