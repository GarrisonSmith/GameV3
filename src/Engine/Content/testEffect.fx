#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

float4x4 worldViewProjection;
sampler TextureSampler : register(s0);

struct VertexInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float4 TextureCordinate : TEXCOORD0;
};

struct PixelInput
{
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float4 TextureCordinate : TEXCOORD0;
};

PixelInput SpriteVertexShader(VertexInput v)
{
    PixelInput output;

    output.Position = mul(v.Position, worldViewProjection);
    output.Color = v.Color;
    output.TextureCordinate = v.TextureCordinate;
    return output;
}
float4 SpritePixelShader(PixelInput p) : SV_TARGET
{
    float darkeningAmount = .3f;
    float4 diffuse = tex2D(TextureSampler, p.TextureCordinate.xy);
    if (p.Position.x > 500)
    {
        p.Color.rgb *= darkeningAmount;
    }
    
    return diffuse * p.Color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}