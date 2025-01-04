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
float2 uNoiseScale;
float uPower;
float uNoiseSpeed;
float uNoiseStrength;
float4 uDarkColor;
float4 uGlowColor;

float4 PixelShaderFunction(float4 baseColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 randomOffset = float2(baseColor.r, baseColor.g);
    float progress = clamp(baseColor.b, 0, 1);
    float4 noise = tex2D(tex0, (coords + randomOffset) * uNoiseScale - float2(progress * uNoiseSpeed, 0));
    float4 noise2 = tex2D(tex0, (coords - randomOffset) * uNoiseScale - float2(progress * uNoiseSpeed + (length(noise.rgb) / 3 - 0.5) * 0.5, 0));
    float finalNoise = length(noise2.rgb) / 3;

    float4 finalImage = tex2D(uImage0, coords + float2(finalNoise - 0.5, 0) * uNoiseStrength * (0.1 + progress * 0.9));
    float power = pow(finalImage + progress * 0.4, uPower * progress + 0.01);
    return finalImage.a * lerp(uDarkColor, uGlowColor, length(finalImage.rgb) / 3 * (clamp(power, 1 - progress, 1) + 1 - progress * 1.5)) * smoothstep(1.0, 0.3, progress);

}
technique Technique1
{
    pass PixelShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
