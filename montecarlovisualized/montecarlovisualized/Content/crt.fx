#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

int uTime;

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

float2 curve(float2 uv, float strength)
{
	uv = (uv - 0.5) * 2.0;
	uv *= 1.1;	

	uv.x *= 1.0 + (pow(abs(uv.y) / 5.0, strength));
	uv.y *= 1.0 + (pow(abs(uv.x) / 4.0, strength));

	uv = (uv/2.0) + 0.5;
	uv = (uv*0.92) + 0.04;

	return uv;
}

float mod(float x, float y)
{
	return x - (y * floor(x / y));
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 uv = curve(input.TextureCoordinates, 2.25);
	float3 oricol = tex2D(SpriteTextureSampler, input.TextureCoordinates).rgb;
	float3 col;
	float x = sin((0.3 * uTime) + (uv.y * 21.0)) * sin((0.7 * uTime) + (uv.y * 29.0)) * sin(0.3 + (0.33 * uTime) + (uv.y * 31.0)) * 0.00017;

	col.r = tex2D(SpriteTextureSampler, float2(x + uv.x + 0.001, uv.y + 0.001)).r + 0.05;
	col.g = tex2D(SpriteTextureSampler, float2(x + uv.x + 0.000, uv.y - 0.002)).g + 0.05;
	col.b = tex2D(SpriteTextureSampler, float2(x + uv.x - 0.002, uv.y + 0.000)).b + 0.05;
	col.r += 0.03 * tex2D(SpriteTextureSampler, (0.75 * float2(x+0.025, -0.027)) + float2(uv.x + 0.001, uv.y + 0.001)).r;
	col.g += 0.02 * tex2D(SpriteTextureSampler, (0.75 * float2(x+-0.022, -0.02)) + float2(uv.x + 0.000, uv.y - 0.002)).g;
	col.b += 0.03 * tex2D(SpriteTextureSampler, (0.75 * float2(x+-0.02, -0.018)) + float2(uv.x - 0.002, uv.y + 0.000)).b;

	col = clamp((col * 0.6) + (col * col * 0.4), 0.0, 1.0);

	float vig = 16.0 * uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
	float viginp = pow(abs(vig), 0.35);
	col *= float3(viginp, viginp, viginp);

	col *= float3(1.01, 1.05, 1.02);
	col *= 6.4;

	int vptHeight = 720;
	float scans = clamp(0.35+0.35*sin((3.5 * uTime) + (uv.y * vptHeight * 1.5)), 0.0, 1.0);
	float s = pow(abs(scans), 1.7);
	float sinp = 0.4 + (0.7 * s);
	col *= sinp;

	if (uv.x < 0.0 || uv.x > 1.0)
		col *= 0.0;
	if (uv.y < 0.0 || uv.y > 1.0)
		col *= 0.0;

	col *= 1.0 - (0.65 * clamp((mod(input.TextureCoordinates.x, 2.0)-1.0) * 2.0, 0.0, 1.0));

	return float4(col.rgb, 1.0) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};