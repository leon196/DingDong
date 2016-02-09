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
			float _GrayCount;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				float2 uv = i.uv;
				float2 pixelUV = floor(uv * _Resolution) / _Resolution;

				uv = lerp(uv, pixelUV, step(0.75, _LightRatio));

				fixed4 color = tex2D(_MotionTexture, i.uv);
				fixed4 webcam = tex2D(_WebcamTexture, pixelUV);

				webcam.rgb = fixed3(1,1,1) * floor(Luminance(webcam) * _GrayCount) / _GrayCount;

				color.rgb = min(1, color.rgb + webcam.rgb * 0.5);

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
