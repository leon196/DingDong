﻿Shader "Hidden/Game" {
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
			sampler2D _WebcamTexture;
			sampler2D _WebcamTextureLast;
			sampler2D _MotionTexture;
			sampler2D _PostProcessTexture;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				return tex2D(_MotionTexture, i.uv);
			}
			ENDCG
		}
	}
}