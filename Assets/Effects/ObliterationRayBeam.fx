sampler uImage0 : register(s0);

texture uTexture0;
sampler2D a1 = sampler_state
{
    texture = <uTexture0>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};
texture uTexture1;
sampler2D a2 = sampler_state
{
    texture = <uTexture1>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

float uTime;
float4 uGlowColor;
matrix transformMatrix;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 Coord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 Coord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    output.Position = mul(input.Position, transformMatrix);
    output.Color = input.Color;
    output.Coord = input.Coord;
    return output;
}

float4 PixelShaderFunction(in VertexShaderOutput input) : COLOR0
{
    float2 coords = input.Coord;
    coords.y = (input.Coord.y - 0.5) / input.Coord.z + 0.5;
        
    float4 n1 = tex2D(a1, coords + float2(-uTime, 0)) + tex2D(a2, coords + float2(-uTime * 2, 0));
    return saturate(pow(length(n1.rgb) / 2.5, 4)) * input.Color + (pow(length(n1.rgb) / 2, 2) + length(n1.rgb) * 0.1) * uGlowColor;;
}

technique Technique1
{
    pass ShaderPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}