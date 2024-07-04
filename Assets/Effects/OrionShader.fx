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
    
    float2 offsetCoords = float2(uWorldPosition.x * uDirection, uWorldPosition.y + uTime * 200) * 0.03;
    float noise = length((tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + float2(0, uTime * 4) * 2) / uImageSize1) * tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + float2(0, uTime * 4)) / uImageSize1)).rgb);
    float noise2 = length((tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + float2(0, uTime * 2) * 4) / uImageSize1 / 2) * tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + float2(0, uTime * 2) * 3) / uImageSize1 * 2)).rgb);
    float space = length((tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + offsetCoords * 2) / uImageSize1) * tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + offsetCoords) / uImageSize1)).rgb);
    
    float2 spaceCoords = input + float2((noise) * 0.05, ((noise2 - 0.2) * 0.05));
    float flames;
    float luminosity = (color.r + color.g + color.b);
    
    float yOff = round((noise2 + noise2 * noise) * 20);
    float edge = length(tex2D(uImage0, resize(input, float2(noise * 5 - 1, yOff))).rgb) / 3;
    float edge2 = length(tex2D(uImage0, resize(input, float2(noise2 * 2, 0))).rgb + tex2D(uImage0, resize(input, float2(noise * 2, 2))).rgb) / 3;
    
    float4 finalImage = float4(pow(space * 4, 4) * noise2 * (uColor + 0.2) * 2 * luminosity + pow(luminosity, 2) * uSecondaryColor * 0.05, 1) * color.a * base;

    if ((edge + edge2) > 0.1 && length(color) == 0)
    {
        finalImage += float4(uColor * (space * 3 + 0.5), space) + float4(uSecondaryColor * (space * 2), space * 0.5 + 0.5) * base * color.a;
    }
        
    return finalImage;
}

technique Technique1
{
    pass OrionPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}