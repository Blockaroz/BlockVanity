sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float2 uTargetPosition;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 PixelShaderFunction(float4 base : COLOR0, float2 input : TEXCOORD0) : COLOR0
{
    float color = length(tex2D(uImage0, input).rgb) / 1.5 - 0.2;
    float2 fixedInput = (input * uImageSize0 - uSourceRect.xy) / uSourceRect.zw;

    float wave = (sin(-fixedInput.y * 2 - pow(fixedInput.x - 0.5, 2) * 5 + color * 4 + uTime * 3.14) + 1.0) / 2.0;
    float4 colorMap = tex2D(uImage1, clamp(float2(pow(color, 2), 1 - wave), 0.01, 0.99));

    return colorMap * 1.2 * base * (color > 0 ? 1 : 0);

}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}