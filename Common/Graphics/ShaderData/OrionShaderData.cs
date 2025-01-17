using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics;

public class OrionShaderData : ArmorShaderData
{
    public OrionShaderData() : base(AllAssets.Effects.OrionDye, "ShaderPass") { }

    public void UseColors(Color glow, Color secondary, Color fade)
    {
        _color0 = glow;
        _color1 = secondary;
        _color2 = fade;
    }

    private Color _color0;
    private Color _color1;
    private Color _color2;

    public override void Apply(Entity entity, DrawData? drawData = null)
    {
        base.Apply(entity, drawData);
        UseImage(AllAssets.Textures.OrionNoise);
        Shader.Parameters["uDirection"].SetValue(entity is null ? 1 : entity.direction);
        Shader.Parameters["uColor"].SetValue(_color0.ToVector3());
        Shader.Parameters["uSecondaryColor"].SetValue(_color1.ToVector3());
        Shader.Parameters["uTertiaryColor"].SetValue(_color2.ToVector3());

        Apply();
    }
}