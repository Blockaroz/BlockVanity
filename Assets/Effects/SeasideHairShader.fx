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

float2 resize(float2 coords, float2 offset)
{
    return ((coords * uImageSize0) + offset) / uImageSize0;
}

float4 PixelShaderFunction(float4 base : COLOR0, float2 input : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, input);
    float baseAlpha = length(base) / 4 > 0 ? 1 : 0;

    return color;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}