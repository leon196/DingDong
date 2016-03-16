
// Credits goes to Hornet
// https://www.shadertoy.com/view/ltBSRG

// This set suits the coords of of 0-1.0 ranges..
#define MOD3 float3(443.8975,397.2973, 491.1871)
#define NUM_LEVELS_F 64.0

float hash12 (float2 p) {
	float3 p3  = frac(float3(p.xyx) * MOD3);
	p3 += dot(p3, p3.yzx + 19.19);
	return frac(p3.x * p3.z * p3.y);
}

float dither (float x, float2 seed) {
	x += (hash12(seed) + hash12(seed + 3.1337) - 0.5) / 255.0;
	return x;
}

float render (float2 uv, float lum) {
	float time = 0.;

	float v = lum / NUM_LEVELS_F;

	float2 vseed = lum;
	v = dither(v, vseed);
	v = floor(v * 255.0) / 255.0; //quantisation to 8bit

	return v * NUM_LEVELS_F;
}
