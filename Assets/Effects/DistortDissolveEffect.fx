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
    float4 noise = tex2D(tex0, (coords + randomOffset) * uNoiseScale - float2(progress * uNoiseSpeed, 0));
    float4 noise2 = tex2D(tex0, (coords - randomOffset) * uNoiseScale - float2(progress * uNoiseSpeed, 0));
    float finalNoise = length(noise2.rgb) / 3;

    float4 regularImage = tex2D(uImage0, coords);
    float4 finalImage = tex2D(uImage0, coords - float2(finalNoise, 0) * uNoiseStrength * sqrt(progress) * (1 - length(regularImage.rgb) / 3));
    
    float powerNoise = length(tex2D(tex0, (coords + randomOffset) * 2 * uNoiseScale).rgb) / 3;
    float power = clamp(pow(finalImage * (powerNoise + 0.9) * smoothstep(1.8, 0.4, progress), uPower * progress + 1), 0, 1);
    
    float altLerp = smoothstep(0.05, 0.3, power * pow(progress, 4)) * smoothstep(0.3, 0.5, finalNoise);
    float4 color = lerp(lerp(uDarkColor, uGlowColor, power * power * length(finalImage.rgb) / 2 * smoothstep(1.2, 0.4, progress)), uAltColor, altLerp);
    return length(finalImage.rgb) / 1.5 * smoothstep(1.0, 0.8, progress - power * 0.1) * color;

}
technique Technique1
{
    pass PixelShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
