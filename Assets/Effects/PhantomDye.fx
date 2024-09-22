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
    
    float2 pixelInput = floor((float2(fixedInput.x * uSourceRect.z, (pow(fixedInput.y, 1.5) + uTime) * uImageSize0.y / 4) - uSourceRect.xy) / 2) / uImageSize0 * 2;
    float2 pixelInput2 = floor((float2(fixedInput.x * uSourceRect.z, (pow(fixedInput.y, 1.5) + uTime * 4) * uImageSize0.y / 4) - uSourceRect.xy) / 2) / uImageSize0 * 2;
    
    float4 noise = tex2D(uImage1, pixelInput);
    float4 noise2 = tex2D(uImage1, pixelInput2);
    
    float noiseSparkles = pow(noise.r * noise2.g + noise.g * noise2.b, 2) * 2;
    float fadeDown = smoothstep(1.1, 0.4, fixedInput.y - noiseSparkles * 0.3);
        
    float4 color = tex2D(uImage0, input);
    color += tex2D(uImage0, input + float2((noise.g / uSourceRect.z) * 0.015, noise.r * noise2.b / uImageSize0.y * 10)) * 0.1;
    
    float lightness = (color.r + color.g + color.b) / 3;

    float3 finalColor = (lerp(uSecondaryColor * (lightness + 0.2), uColor * (lightness * 1.5 + 0.4) + pow(lightness, 1.6) * 0.8, pow(1 - fixedInput.y, 1.5) * (lightness * 0.4 + 0.6) + noiseSparkles * 0.2) + pow(noiseSparkles, 2) * fixedInput.y) * (length(color) > 0 ? 1 : 0);
    return float4(finalColor * 1.5 * fadeDown * (base.rgb * 0.8 + 0.25), color.a) * base.a * smoothstep(-0.2, 0.3, length(finalColor) / 4 + noiseSparkles * 0.1);
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}