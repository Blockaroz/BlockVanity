using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace BlockVanity.Common.UI;

public class UIParticleSystem : UIElement
{
    public ParticleSystem particles;

    public UIParticleSystem()
    {
        IgnoresMouseInteraction = true;
        particles = new ParticleSystem();
        base.OnUpdate += ParticleSystemUpdate;
    }

    private void ParticleSystemUpdate(UIElement affectedElement)
    {
        particles.Update();
    }

    public override void Recalculate()
    {
        base.Recalculate();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        particles.Draw(spriteBatch, Vector2.Zero, BlendState.AlphaBlend, Main.UIScaleMatrix);
    }
}
