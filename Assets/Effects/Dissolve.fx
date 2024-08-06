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
float2 uTextureScale;
float uFrameCount;
float4 uSourceRect;
float uProgress;
float uPower;
float uNoiseStrength;
float uRotation;

float4 PixelShaderFunction(float4 baseColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 center = (float2(coords.x, coords.y * uFrameCount - uSourceRect.y / uSourceRect.w) - 0.5) * uTextureScale;
    float cosine = cos(uRotation);
    float sine = sin(uRotation);
    float2 unRotated = float2(center.x * cosine - center.y * sine, center.y * cosine + center.x * sine) + float2(0, uProgress * 1.2);
    float2 baseDistortion = (length(tex2D(tex0, float2(unRotated.x / 2, unRotated.y / 2 - uProgress * 2) * uTextureScale).rgb) / 3 - 0.5) * length(coords * 2 - 1);

    float noiseCoord = (length(tex2D(tex0, float2(unRotated.x / 2, unRotated.y / 2 - uProgress) + baseDistortion * 0.2 * uProgress).rgb) / 3 - 0.5) * (coords - 0.5) * 2;
    
    float4 original = tex2D(uImage0, coords - baseDistortion * 0.1 * uProgress);

    float4 noise = tex2D(tex0, unRotated + noiseCoord * 0.2) * original;
    float power = pow(original * (1.05 + noise * uNoiseStrength * 0.33 - uProgress * (0.5f + uNoiseStrength)), uPower);
    return power * baseColor;
}
technique Technique1
{
    pass PixelShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}