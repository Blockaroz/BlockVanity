sampler uImage0 : register(s0);
texture uTexture;
sampler tex0 = sampler_state
{
    texture = <uTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
float uFrameCount;
float2 uNoiseScale;
float uPower;
float uNoiseSpeed;
float uNoiseStrength;
float4 uDarkColor;
float4 uGlowColor;
float4 uAltColor;

float4 PixelShaderFunction(float4 baseColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 randomOffset = float2(baseColor.r, baseColor.g);
    float progress = clamp(baseColor.b, 0, 1);
    float4 noise = tex2D(tex0, (coords + randomOffset) * uNoiseScale - float2(0, progress * uNoiseSpeed));
    float finalNoise = length(noise.rgb) / 3.0;

    float4 regularImage = tex2D(uImage0, coords);
    float2 uvOffset = float2(0, (finalNoise - 0.25) / uFrameCount) * uNoiseStrength * sqrt(progress * 0.9 + 0.1);
    float4 finalImage = tex2D(uImage0, coords + uvOffset);
        
    float imageStrength = length(finalImage.rgb) / 3;
    float4 multColor = lerp(uGlowColor, uAltColor, smoothstep(0.7, 0.8, progress + (finalNoise - 0.25) * 0.1 + 0.2 - imageStrength * 0.4));
    float4 color = lerp(uDarkColor, multColor, smoothstep(0.1, 0.3, pow(imageStrength + 0.15, uPower + 1))) * finalImage.a;
    return color * smoothstep(1.0, 0.75, progress);

}
technique Technique1
{
    pass PixelShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
