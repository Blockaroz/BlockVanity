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

float2 rotate(float2 coords)
{
    return (coords.x * cos(uRotation) - coords.y * sin(uRotation), coords.x * sin(uRotation) - coords.y * cos(uRotation));
}

float4 PixelShaderFunction(float4 base : COLOR0, float2 input : TEXCOORD0) : COLOR0
{
    float edges[4];
    
    edges[0] = length(tex2D(uImage0, resize(input, float2(0, 2))).rgba) / 4;
    edges[1] = length(tex2D(uImage0, resize(input, float2(0, -2))).rgba) / 4;
    edges[2] = length(tex2D(uImage0, resize(input, float2(2, 0))).rgba) / 4;
    edges[3] = length(tex2D(uImage0, resize(input, float2(-2, 0))).rgba) / 4;
    
    float4 color = tex2D(uImage0, input);
    float baseAlpha = length(base) / 4 > 0 ? 1 : 0;
    
    float2 offsetCoords = (float2(uWorldPosition.x * uDirection, uWorldPosition.y) * 0.1);
    float space = length((tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + offsetCoords * 2) / uImageSize1 * 1.2) * tex2D(uImage1, (input * uImageSize0 - uSourceRect.xy + offsetCoords) / uImageSize1)).rgb);
    if (space > 0.33)
        space += 1;
    
    if ((edges[0] == 0 || edges[1] == 0 || edges[2] == 0 || edges[3] == 0) && length(color.rgba) / 4 > 0)
    {
        return float4(uColor, 0.9) * base.a + space * 1.2;
    }
   
    return float4(uSecondaryColor * uColor * space, 0.99) * base.a * color.a;

}

technique Technique1
{
    pass OrionPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}