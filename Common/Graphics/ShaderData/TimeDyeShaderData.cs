using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics;

public class TimeDyeShaderData : ArmorShaderData
{
    private float _timeScale;

    public TimeDyeShaderData(Asset<Effect> shader, float timeScale = 10) : base(shader, "ShaderPass")
    {
        _timeScale = timeScale;
    }

    public override void Apply(Entity entity, DrawData? drawData = null)
    {
        base.Apply(entity, drawData);
        Shader.Parameters["uTime"]?.SetValue(Main.GlobalTimeWrappedHourly / _timeScale);
        Shader.CurrentTechnique.Passes[0].Apply();
    }
}
