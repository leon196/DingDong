Shader "Hidden/Game" {
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
				// float motion = Luminance(tex2D(_MotionTexture, i.uv));
				// float fadeOut = Luminance(tex2D(_PostProcessTexture, i.uv));
				// fadeOut *= 0.99;

				// fixed4 col = fixed4(1,1,1,1);
				// col.rgb *= lerp(fadeOut, motion, step(0.5, motion));

				return tex2D(_MotionTexture, i.uv);
			}
			ENDCG
		}
	}
}
