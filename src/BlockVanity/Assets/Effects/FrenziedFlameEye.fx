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

float4 uColor;
float4 uSecondaryColor;
float uTime;
float uVertexDistortion;
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
    float2 offset = float2(cos((uTime + input.Coord.x * 0.5 - input.Coord.y * 0.5) * 6.28), sin((uTime + input.Coord.y * 0.5 - input.Coord.x * 0.5) * 6.28));
    output.Position = mul(input.Position + float4(offset * uVertexDistortion, 0, 0), transformMatrix);
    return output;
}

float4 PixelShaderFunction(in VertexShaderOutput input) : COLOR0
{
    float2 centerUV = input.Coord * 2 - 1;
    float2 polarCoords = float2(atan2(centerUV.y, centerUV.x) / 6.28, sqrt(length(centerUV)) / 2);
    
    float4 preImage = tex2D(tex0, polarCoords + float2(0, -frac(uTime)));
    float4 image = tex2D(tex0, polarCoords + float2(0, -frac(uTime) + length(preImage) * 0.2));
    float radialNoise = length(centerUV) + (image - 0.4) * 0.4;
    
    float4 glowingRing = lerp(float4(uColor.rgb * smoothstep(0.4, 0.7, radialNoise), 1), 1, smoothstep(0.5, 0.6, radialNoise));
    float4 bloom = uColor * pow(length(centerUV), 1.2);
    float outerFade = smoothstep(1.0, 0.8, radialNoise);

    return lerp(uSecondaryColor * radialNoise * outerFade, glowingRing + bloom, pow(outerFade, 2));
}

technique Technique1
{
    pass EnergyPass
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}