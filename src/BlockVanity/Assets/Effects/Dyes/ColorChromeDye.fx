sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
texture uNoise;
sampler2D noise = sampler_state
{
    texture = <uNoise>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
float3 uColor;
float3 uSecondaryColor;
float3 uTertiaryColor;
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
    float2 fixedInput = (input * uImageSize0 - uSourceRect.xy) / uSourceRect.zw;
    
    float2 pixelInput = floor((float2(fixedInput.x * uSourceRect.z, (pow(fixedInput.y, 1.3) + uTime * 2) * uSourceRect.w)) / 2) / uSourceRect.zw * 2;
    float2 pixelInput2 = floor((float2(frac(fixedInput.x + 0.5) * uSourceRect.z, (pow(fixedInput.y, 1.5) + uTime * 3) * uSourceRect.w)) / 2) / uSourceRect.zw * 2;
    
    return tex2D(uImage0, pixelInput) * base;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}