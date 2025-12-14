sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
texture uNoise0;
sampler2D noise0 = sampler_state
{
    texture = <uNoise0>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
texture uNoise1;
sampler2D noise1 = sampler_state
{
    texture = <uNoise1>;
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

float FrontEdgeFunction(float2 input)
{
    float4 image = tex2D(uImage0, input);
    if (image.a > tex2D(uImage0, input + float2(2 / uImageSize0.x, 0)).a)
    {
        return 1;
    }
    if (image.a > tex2D(uImage0, input + float2(0, -2 / uImageSize0.y)).a)
    {
        return 1;
    }
    
    return 0;
}

float4 PixelShaderFunction(float4 base : COLOR0, float2 input : TEXCOORD0) : COLOR0
{
    float2 fixedInput = (input * uImageSize0 - uSourceRect.xy) / uSourceRect.zw;
    
    float2 pixelInput = floor((float2((fixedInput.x ) * uSourceRect.z, fixedInput.y * uSourceRect.w)) / 2) / uSourceRect.zw * 2;
    
    float4 grain = tex2D(noise0, pixelInput + float2(pixelInput.y, uTime));
    float4 grain2 = tex2D(noise1, pixelInput);
    grain -= grain2 * 0.8;
        
    float4 image = tex2D(uImage0, input);
    float alpha = base.a * image.a;

    float3 gray = pow(length(image.rgb).xxx, 2.25);
    float frontEdge = FrontEdgeFunction(input);
    
    float silica = smoothstep(0.6, 0.9, grain.x) * (1 - fixedInput.y * alpha);
    
    float backEdge = 0;
    for (int i = 0; i < 4; i++)
    {
        if (image.a > tex2D(uImage0, input - float2((floor(abs(grain2.x) * i * 3) * 2) / uImageSize0.x, 0)).a)
        {
            backEdge += 0.25;
        }
    }
    
    float3 wither = saturate(gray * 0.12 - pow(abs(grain.x), 3) * 0.5 + 0.1) * pow(1 - backEdge, 2);

    return float4(wither * base.rgb + silica.xxx, 1 - backEdge * 0.05) * alpha;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}