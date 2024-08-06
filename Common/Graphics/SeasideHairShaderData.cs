using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics;

public class SeasideHairShaderData : HairShaderData
{
    public SeasideHairShaderData() : base(AllAssets.Effects.SeasideHairShader, "ShaderPass") { }

    public override Color GetColor(Player player, Color lightColor) => lightColor;

    public override void Apply(Player player, DrawData? drawData = null)
    {
        base.Apply(player, drawData);
        UseImage(AllAssets.Textures.SeasideColorMap);
        Shader.Parameters["uTime"]?.SetValue(Main.GlobalTimeWrappedHourly / 2f);
        Shader.CurrentTechnique.Passes[0].Apply();
    }
}
