using BlockVanity.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Dyes;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Dyes;

public class ReflectiveColorChromeDye : ModItem
{
    public static LazyAsset<Effect> ColorChromeDyeEffect { get; } = new LazyAsset<Effect>($"{nameof(BlockVanity)}/Assets/Effects/Dyes/ColorChromeDye");
    public static LazyAsset<Texture2D> ColorChromeMap { get; } = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/ColorChromeMap");

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 3;

        if (!Main.dedServ)
        {
            GameShaders.Armor.BindShader(Type, new ReflectiveAssetShaderData(ColorChromeDyeEffect, "ShaderPass")
                .UseImage(ColorChromeMap));
        }
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.sellPrice(silver: 75);
        Item.rare = ItemRarityID.Green;
    }
}

public sealed class ReflectiveAssetShaderData : ArmorShaderData
{
    public ReflectiveAssetShaderData(Asset<Effect> shader, string passName) : base(shader, passName) { }

    public override void Apply(Entity entity, DrawData? drawData)
    {
        if (entity == null)
        {
            base.Shader.Parameters["uLightSource"]?.SetValue(Vector3.Zero);
        }
        else
        {
            float num = 0f;
            if (drawData.HasValue)
            {
                num = drawData.Value.rotation;
            }

            Vector2 position = entity.position;
            float num2 = entity.width;
            float num3 = entity.height;
            Vector2 vector = position + new Vector2(num2, num3) * 0.1f;
            num2 *= 0.8f;
            num3 *= 0.8f;
            Vector3 subLight = Lighting.GetSubLight(vector + new Vector2(num2 * 0.5f, 0f));
            Vector3 subLight2 = Lighting.GetSubLight(vector + new Vector2(0f, num3 * 0.5f));
            Vector3 subLight3 = Lighting.GetSubLight(vector + new Vector2(num2, num3 * 0.5f));
            Vector3 subLight4 = Lighting.GetSubLight(vector + new Vector2(num2 * 0.5f, num3));
            float num4 = subLight.X + subLight.Y + subLight.Z;
            float num5 = subLight2.X + subLight2.Y + subLight2.Z;
            float num6 = subLight3.X + subLight3.Y + subLight3.Z;
            float num7 = subLight4.X + subLight4.Y + subLight4.Z;
            Vector2 spinningpoint = new Vector2(num6 - num5, num7 - num4);
            float num8 = spinningpoint.Length();
            if (num8 > 1f)
            {
                num8 = 1f;
                spinningpoint /= num8;
            }

            if (entity.direction == -1)
            {
                spinningpoint.X *= -1f;
            }

            spinningpoint = spinningpoint.RotatedBy(0f - num);
            Vector3 value = new Vector3(spinningpoint, 1f - (spinningpoint.X * spinningpoint.X + spinningpoint.Y * spinningpoint.Y));
            value.X *= 2f;
            value.Y -= 0.15f;
            value.Y *= 2f;
            value.Normalize();
            value.Z *= 0.6f;
            base.Shader.Parameters["uLightSource"]?.SetValue(value);
        }

        base.Apply(entity, drawData);
    }
}