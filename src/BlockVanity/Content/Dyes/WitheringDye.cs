using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dyes;

public sealed class WitheringDye : ModItem
{
    public static LazyAsset<Effect> WitheringDyeEffect { get; } = new LazyAsset<Effect>($"{nameof(BlockVanity)}/Assets/Effects/Dyes/WitheringDye");

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
    public WitheringDyeShaderData() : base(WitheringDye.WitheringDyeEffect, "ShaderPass") { }

    public override void Apply(Entity entity, DrawData? drawData = null)
    {
        base.Apply(entity, drawData);

        Shader.Parameters["uTime"]?.SetValue(Main.GlobalTimeWrappedHourly * 0.2f);
        Shader.Parameters["uNoise0"]?.SetValue(Assets.Textures.MiscNoise[1].Value);
        Shader.Parameters["uNoise1"]?.SetValue(Assets.Textures.MiscNoise[3].Value);

        Shader.CurrentTechnique.Passes[0].Apply();
    }
}