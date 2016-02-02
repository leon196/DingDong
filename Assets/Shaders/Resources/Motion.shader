Shader "Hidden/Motion" {
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
			sampler2D _CurrentWebcamTexture;

			fixed4 frag (v2f_img i) : SV_Target {
				fixed4 col = tex2D(_CurrentWebcamTexture, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
