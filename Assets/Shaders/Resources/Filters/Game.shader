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

			float _MinX;
			float _MaxX;
			float _MinY;
			float _MaxY;

			fixed4 frag (v2f_img i) : SV_Target 
			{
				float2 uv = i.uv;

				uv = lerp(uv, floor(uv * _Resolution) / _Resolution, step(0.75, _LightRatio));

				fixed4 color = tex2D(_MotionTexture, i.uv);

				color.rgb *= _LightRatio;

				// Spawn rect
				float2 thinkness = float2(0.001, 0.001);
				thinkness.x *= _Resolution.x / _Resolution.y;
				float x =  step(_MinX, uv.x) * step(uv.x, _MaxX);
				float y =  step(_MinY, uv.y) * step(uv.y, _MaxY);
				float rect = step(_MinX, uv.x) * step(uv.x, _MinX + thinkness) * y;
				rect += step(_MaxX, uv.x) * step(uv.x, _MaxX + thinkness) * y;
				rect += step(_MinY, uv.y) * step(uv.y, _MinY + thinkness) * x;
				rect += step(_MaxY, uv.y) * step(uv.y, _MaxY + thinkness) * x;
				rect = clamp(rect, 0, 1);
				color.rgb = lerp(color.rgb, float3(1,1,1), rect * step(_LightRatio, 0.75));

				// Layer GUI
				fixed4 gui = tex2D(_GUITexture, uv);
				color.rgb = lerp(color.rgb, gui.rgb, gui.a);

				return color;
			}
			ENDCG
		}
	}
}
