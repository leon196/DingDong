Shader "Unlit/Fullscreen"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			o.Emission = tex2D (_MainTex, screenUV).rgb;
		}
		ENDCG
	} 
	Fallback "Diffuse"
}