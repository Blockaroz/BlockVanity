sampler uImage0 : register(s0);

texture uStreak0;
sampler2D a1 = sampler_state
{
    texture = <uStreak0>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
texture uStreak1;
sampler2D a2 = sampler_state
{
    texture = <uStreak1>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
float3 uColor;
float uOpacity;
float uTime;
float uGlow;
matrix transformMatrix;

struct VertexShaderInput
{
    float2 Coord : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float2 Coord : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    output.Color = input.Color;
    output.Coord = input.Coord;
    output.Position = mul(input.Position, transformMatrix);
    return output;
}

float4 PixelShaderFunction(in VertexShaderOutput input) : COLOR0
{
    float4 n1 = tex2D(a1, input.Coord + float2(-uTime, 0)) * (1 - input.Coord.x);
    float4 n2 = tex2D(a2, input.Coord + float2(-uTime * 3, 0)) * (1 - input.Coord.x) * (1 - abs(input.Coord.y * 2 - 1));
    float4 trail = (pow(n2, 2) * 3 + n1) > 0.2 ? float4(1, 1, 1, 0) : 0;
    if (length(trail) > 0)
        trail.a = 0.9;
    float4 bloom = (1 - input.Coord.x * 1.3) * (1 - abs(input.Coord.y * 2 - 1)) * uGlow;
    bloom.a /= 2;
    return (trail + bloom) * input.Color * uOpacity;

}

technique Technique1
{
    pass PrismPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}