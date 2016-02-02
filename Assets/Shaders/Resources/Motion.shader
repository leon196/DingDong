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
			sampler2D _MotionTexture;
			sampler2D _WebcamTexture;
			sampler2D _WebcamTextureLast;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				fixed4 col = fixed4(1,1,1,1);

				float current = Luminance(tex2D(_WebcamTexture, i.uv));
				float last = Luminance(tex2D(_WebcamTextureLast, i.uv));

				float motion = step(0.1, abs(current - last));

				float fadeOut = Luminance(tex2D(_MotionTexture, i.uv));
				fadeOut *= 0.99;

				col.rgb *= lerp(fadeOut, motion, step(0.5, motion));

				return col;
			}
			ENDCG
		}
	}
}
