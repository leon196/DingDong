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
			sampler2D _GUITexture;

			float _LightRatio;
			float2 _Resolution;
			float2 _BonusPosition;
			float _BonusSize;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				float2 uv = i.uv;

				uv = lerp(uv, floor(uv * _Resolution) / _Resolution, step(_LightRatio, 0.0));

				fixed4 color = tex2D(_MotionTexture, i.uv);

				color.rgb *= _LightRatio;

				// Layer GUI
				fixed4 gui = tex2D(_GUITexture, uv);
				color.rgb = lerp(color.rgb, gui.rgb, gui.a);

				return color;
			}
			ENDCG
		}
	}
}
