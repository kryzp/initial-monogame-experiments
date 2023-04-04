#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

int uTime;
int uFadeTime;
int uWindowHeight;
Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float mod(float x, float y)
{
	return x - (y * floor(x / y));
}

float sigmoid(float a, float x)
{
    return 1.0 - (2.0 / (1.0 + exp(a * x)));
}

float washwave(float t)
{
    return exp(-0.45 * t * t);
}

float washwave_periodic(float t)
{
    const float PERIOD = 30.0;
    return washwave(((t + (PERIOD / 2.0)) % PERIOD) - (PERIOD / 2.0));
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // adding the vignette here could obscure vision - neat gameplay mechanic tradeoff!!!!

    float2 uv = input.TextureCoordinates;
    float3 col = tex2D(SpriteTextureSampler, uv).rgb;

	float vig = 16.0 * uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
	float viginp = pow(abs(vig), 0.25);
	col *= float3(viginp, viginp, viginp);

	col *= float3(1.074, 2.55, 1.01);
	col *= clamp(0.5 + sigmoid(5.0, float(uFadeTime) / 1000.0), 0.5, 1.1);
	
	float scans = clamp(0.35+0.35*sin((3.5 * uTime) + (uv.y * uWindowHeight * 1.5)), 0.0, 1.0);
	float s = pow(abs(scans), 1.7);
	float sinp = 0.4 + (0.7 * s);
	col *= sinp;

	col *= 1.0 - (0.65 * clamp((mod(input.TextureCoordinates.x, 2.0)-1.0) * 2.0, 0.0, 1.0));
	
	col *= 1.0 + (washwave_periodic((uTime * 0.005) - input.TextureCoordinates.y * 10.0) * 0.75);

	return float4(col.rgb, 1.0) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
