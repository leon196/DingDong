Shader "Hidden/Webcam" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
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

			fixed4 frag (v2f_img i) : SV_Target {
				fixed4 col = tex2D(_TextureWebcam, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
