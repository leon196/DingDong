Shader "Hidden/Fade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			// sampler2D _TextureWebcam;
			sampler2D _TextureDifference;
			sampler2D _TextureChoupichoup;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				// half4 webcam = tex2D(_TextureWebcam, uv);
				half4 difference = tex2D(_TextureDifference, uv);
				half4 choupichoup = tex2D(_TextureChoupichoup, uv);
				half4 color = max(difference, choupichoup);
				return color;
			}
			ENDCG
		}
	}
}
