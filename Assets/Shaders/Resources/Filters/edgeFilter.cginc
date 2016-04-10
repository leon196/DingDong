
half4 edgeFilter (sampler2D bitmap, float2 uv, float2 dimension)
{
  half4 color = half4(0.0, 0.0, 0.0, 0.0);

  color += -1.0 * tex2D(bitmap, uv + float2(-2, -2) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-2, -1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-2,  0) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-2,  1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-2,  2) / dimension);

  color += -1.0 * tex2D(bitmap, uv + float2(-1, -2) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-1, -1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-1,  0) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-1,  1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2(-1,  2) / dimension);

  color += -1.0 * tex2D(bitmap, uv + float2( 0, -2) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 0, -1) / dimension);
  color += 24.0 * tex2D(bitmap, uv + float2( 0,  0) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 0,  1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 0,  2) / dimension);

  color += -1.0 * tex2D(bitmap, uv + float2( 1, -2) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 1, -1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 1,  0) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 1,  1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 1,  2) / dimension);

  color += -1.0 * tex2D(bitmap, uv + float2( 2, -2) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 2, -1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 2,  0) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 2,  1) / dimension);
  color += -1.0 * tex2D(bitmap, uv + float2( 2,  2) / dimension);

  return color;
}