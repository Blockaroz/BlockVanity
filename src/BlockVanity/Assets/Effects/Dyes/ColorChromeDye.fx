sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

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

// Reconstructed from decompiled reflective dyes
float4 ArmorReflectiveColor(float4 base, float2 uv)
{
    float4 c4 = float4(0.0f, 0.333333343f, 1.0f, -0.300000012f);
    float4 c5 = float4(1.53846157f, -0.461538464f, -2.0f, 3.0f);
    
    // Create offset images
    float4 nMap = tex2D(uImage0, uv - float2(2 / uImageSize0.x, 0));
    float4 backward = tex2D(uImage0, uv + float2(2 / uImageSize0.x, 0));
    float4 upward = tex2D(uImage0, uv + float2(0, 2 / uImageSize0.y));
    float4 downward = tex2D(uImage0, uv - float2(0, 2 / uImageSize0.y));
    float4 result = tex2D(uImage0, uv);
        
    nMap.xyz = nMap.xyz - backward.xyz;
    upward.w = nMap.x + nMap.y + nMap.z;
    nMap.x = upward.w * c4.y;
    backward.xyz = -upward.xyz + downward.xyz;
    nMap.w = backward.x + backward.y + backward.z;
    nMap.y = nMap.w * c4.y;

    nMap.w = c4.z - (nMap.x * nMap.x + nMap.y * nMap.y);

    backward.x = 1.0f / sqrt(nMap.w);
    backward.x = 1.0f / backward.x;

    nMap.z = (nMap.w > 0) ? backward.x : c4.x;

    // Apply lighting
    nMap.x = dot(nMap.xyz, uLightSource);
    nMap.y = (nMap.x + c4.w) * c5.x;
    nMap.x = saturate((nMap.x > 0) ? nMap.y : c5.y);
    nMap.y = nMap.x * c5.z + c5.w;
    nMap.x = pow((nMap.x * nMap.x * nMap.y) * 2, 2) * result.w;
    nMap.y = (result.x + result.y + result.z) * c4.y;
    return nMap;
    // Colorize
    nMap.yzw = nMap.y;

    backward.xyz = nMap.wzyx * 0.5;
    result.rgb = nMap.x * nMap.wzy + backward.xyz;

    return result;
}

float4 PixelShaderFunction(float4 base : COLOR0, float2 input : TEXCOORD0) : COLOR0
{
    float2 fixedInput = (input * uImageSize0 - uSourceRect.xy) / uSourceRect.zw;
    
    float4 image = tex2D(uImage0, input);
        
    float4 shine = ArmorReflectiveColor(base, input);

    float gradLight = clamp(shine.r * 0.7 + 0.1, 0.01, 0.99);
    float gradColor = clamp(smoothstep(0.2, 0.9, fixedInput.y) * 0.2 + (1 - gradLight) * 0.8, 0.01, 0.99);
    float3 gradient = tex2D(uImage1, float2(gradLight, gradColor));
    
    return float4(pow(length(image.rgb), 0.7) * 1.2 * gradient * base.rgb, base.a) * image.a;
}

technique Technique1
{
    pass ShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}