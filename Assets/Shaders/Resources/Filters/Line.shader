Shader "Hidden/Line" {
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

			fixed4 frag (v2f_img i) : SV_Target 
			{
				fixed4 color = fixed4(1,1,1,1);
				return color;
			}
			ENDCG
		}
	}
}
