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

			float _LightRatio;
			float2 _Resolution;
			float2 _BonusPosition;
			float _BonusSize;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				fixed4 color = tex2D(_MotionTexture, i.uv);

				float2 position = _BonusPosition - i.uv;
				position.x *= _Resolution.x / _Resolution.y;
				float circle = step(length(position), _BonusSize);
				color.r = min(1., color.r + circle);

				color.rgb *= _LightRatio;

				return color;
			}
			ENDCG
		}
	}
}
