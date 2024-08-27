sampler uImage0 : register(s0);
texture uTexture0;
sampler tex0 = sampler_state
{
    texture = <uTexture0>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
texture uTexture1;
sampler tex1 = sampler_state
{
    texture = <uTexture1>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
float2 uTextureScale;
float uFrameCount;
float4 uSourceRect;
float uProgress;
float uPower;
float uNoiseStrength;

float4 PixelShaderFunction(float4 baseColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 center = (float2(coords.x, coords.y * uFrameCount - uSourceRect.y / uSourceRect.w) - 0.5) * uTextureScale;
    float4 noise0 = tex2D(tex0, float2(center.x / 2, center.y / 2 + uProgress * 0.3 * uNoiseStrength));
    float4 noise1 = tex2D(tex1, float2(center.x / 2, center.y / 2 + uProgress * 0.5 * uNoiseStrength));
    float finalNoise = length(min(noise0, noise1).rgb) / 1.5;
    
    float4 original = tex2D(uImage0, coords);

    float power = pow((finalNoise + uProgress) * (1 - uProgress * 0.6), uPower);
    return original * power * baseColor;
}
technique Technique1
{
    pass PixelShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}