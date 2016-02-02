Shader "Custom/LastFrame" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Cull off
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows keepalpha
		#pragma exclude_renderers d3d11 xbox360
		#pragma target 3.0
		#include "UnityCG.cginc"             

		sampler2D _MainTex;
		sampler2D _LastWebcamTexture;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_LastWebcamTexture, IN.screenPos.xy / IN.screenPos.w);
			o.Emission = c.rgb;
			o.Metallic = 0.;
			o.Smoothness = 0.;
			o.Alpha = 1.;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
