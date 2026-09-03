using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dyes;

public sealed class WitheringDye : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
            GameShaders.Armor.BindShader(Type, new WitheringDyeShaderData());
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(gold: 1, silver: 50);
        Item.rare = ItemRarityID.Orange;
    }
}

public sealed class WitheringDyeShaderData : ArmorShaderData
{
    public WitheringDyeShaderData() : base(Assets.Effects.Dyes.WitheringDye.Asset, "ShaderPass")
    {
        uNoise0 = Shader.GetParameter<Texture2D>("uNoise0");
        uNoise1 = Shader.GetParameter<Texture2D>("uNoise1");
    }

    private readonly EffectParameter<Texture2D> uNoise0;
    private readonly EffectParameter<Texture2D> uNoise1;

    public override void Apply(Entity entity, DrawData? drawData = null)
    {
        base.Apply(entity, drawData);

        uTime.SetValue(Main.GlobalTimeWrappedHourly * 0.2f);
        uNoise0.SetValue(Assets.Textures.Extras.Noise_1.Asset.Value);
        uNoise1.SetValue(Assets.Textures.Extras.Noise_3.Asset.Value);

        Shader.CurrentTechnique.Passes[0].Apply();
    }
}