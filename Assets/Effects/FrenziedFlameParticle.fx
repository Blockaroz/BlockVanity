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
float2 uFrameCount;
float uPower;
float4 uDarkColor;
float4 uGlowColor;
float4 uAltColor;

float4 PixelShaderFunction(float4 baseColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float progress = clamp(baseColor.b, 0, 1);
    float random = baseColor.r - 0.5;
    
    float4 image = tex2D(uImage0, coords);
        
    float imageStrength = length(image.rgb) / 3;
    float4 color = lerp(uDarkColor, uGlowColor, smoothstep(0, 0.23, pow(imageStrength, uPower))) * image.a;
    float4 multColor = lerp(color, color * uAltColor, smoothstep(0.7, 0.8, progress + random * 0.05));
    return multColor * smoothstep(1.0, 0.9, progress);

}
technique Technique1
{
    pass PixelShaderPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
