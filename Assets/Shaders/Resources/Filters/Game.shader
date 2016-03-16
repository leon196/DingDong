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
			sampler2D _MotionTexture;
			sampler2D _GUITexture;

			float _ShowWebcam;
			float _LightRatio;
			float2 _Resolution;
			float4 _GUIColor;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				float2 uv = i.uv;
				uv = lerp(uv, floor(uv * _Resolution) / _Resolution, step(0.75, _LightRatio));

				fixed4 color = tex2D(_MotionTexture, uv);
				fixed4 webcam = tex2D(_WebcamTexture, uv);

				color.rgb = lerp(color.rgb, min(1, color.rgb + webcam.rgb * 0.5), _ShowWebcam);
				color.rgb *= _LightRatio;

				// fixed4 gui = tex2D(_GUITexture, uv);
				// color.rgb = lerp(color.rgb, gui.rgb * _GUIColor.rgb, gui.a * _GUIColor.a);

				return color;
			}
			ENDCG
		}
	}
}
