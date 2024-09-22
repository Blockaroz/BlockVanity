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
    float4 color = tex2D(uImage0, input);
    float lightness = (color.r + color.g + color.b) / 3;
    
    float maxC = max(color.r, max(color.g, color.b));
    float minC = min(color.r, min(color.g, color.b));  
    float saturation = (maxC - minC) / (1. - abs(2 * clamp(lightness, 0, 1.5) - 1));
    
    float3 radColor = lerp(uSecondaryColor * base.rgb, uColor, smoothstep(0, 0.6, lightness) * (0.9 + sin(uTime * 3.14) * 0.1)) * (1.2 + sin(uTime * 3.14) * 0.1);
    float3 finalColor = lerp(lightness * base.rgb, pow(radColor * lightness * 1.3, 1.3), clamp(pow(saturation, 2) + sin(uTime * 3.14) * 0.01, 0, 1));
    
    return float4(finalColor, 1) * color.a * base.a;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}