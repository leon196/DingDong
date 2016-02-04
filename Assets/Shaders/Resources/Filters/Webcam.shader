Shader "Hidden/Webcam" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_MirrorX ("Mirror X", Float) = 0
		_MirrorY ("Mirror Y", Float) = 0
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _TextureWebcam;

			float _MirrorX;
			float _MirrorY;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				float2 uv = i.uv.xy;
				uv.x = lerp(uv.x, 1. - uv.x, _MirrorX);
				uv.y = lerp(uv.y, 1. - uv.y, _MirrorY);
				fixed4 col = tex2D(_TextureWebcam, uv);
				return col;
			}
			ENDCG
		}
	}
}
