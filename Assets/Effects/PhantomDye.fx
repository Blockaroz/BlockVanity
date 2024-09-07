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
    float2 fixedInput = (input * uImageSize0 - uSourceRect.xy) / uSourceRect.zw;
    
    const float2 pixelSize = 256;
    float2 pixelInput = round(float2(fixedInput.x / 4, pow(fixedInput.y / 3, 1.75)) * pixelSize) / pixelSize;
    
    float4 noise = tex2D(uImage1, float2(pixelInput.x, pixelInput.y + uTime));
    float lightness = (color.r + color.g + color.b) / 3;
    
    float noiseSparkles = pow(noise.r + noise.g * noise.b * 2, 3) * 0.5;
    float fadeDown = smoothstep(1.1, 0.4, fixedInput.y - noiseSparkles * 0.2);
    
    float3 finalColor = (lerp(uSecondaryColor * (lightness + 0.2), uColor * (lightness * 1.5 + 0.4) + pow(lightness, 1.6) * 0.8, pow(1 - fixedInput.y, 1.5) + noiseSparkles * 0.5) + pow(noiseSparkles, 2) * fixedInput.y) * (length(color) > 0 ? 1 : 0);
    
    return float4(finalColor * 1.5 * fadeDown * (base.rgb * 0.8 + 0.25), color.a) * base.a * smoothstep(-0.08, 0.3, length(finalColor) / 2.5 + noiseSparkles * 0.3);
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}