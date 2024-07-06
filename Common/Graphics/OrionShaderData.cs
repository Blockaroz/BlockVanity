using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics;

public class OrionShaderData : ArmorShaderData
{
    public OrionShaderData() : base(AllAssets.Effects.OrionShader, "OrionPass") { }

    public override void Apply(Entity entity, DrawData? drawData = null)
    {
        base.Apply(entity, drawData);
        UseImage("Images/Misc/noise");
        Shader.Parameters["uDirection"].SetValue(entity is null ? 1 : entity.direction);
        Shader.Parameters["uNoise"].SetValue(AllAssets.Textures.SpaceNoise.Value);

        Shader.CurrentTechnique.Passes[0].Apply();
    }
}
