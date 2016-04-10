Shader "Hidden/Game" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_InvertColor ("Invert Color", Float) = 0
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "edgeFilter.cginc"
			#define PI 3.1415926535897
			
			sampler2D _MainTex;
			sampler2D _WebcamTexture;
			sampler2D _GameTexture;
			sampler2D _MotionTexture;
			sampler2D _GUITexture;

			float _ShowWebcam;
			float _InvertColor;
			float _HurtRatio;
			float _LightRatio;
			float2 _Resolution;
			float4 _GUIColor;

			fixed3 rgbOffset (sampler2D bitmap, float2 uv, float radius, float angle)
			{
				fixed3 rgb = fixed3(0,0,0);
				float3 rgbAngle = float3(angle, PI * 2. / 3. + angle, PI * 4. / 3. + angle);
				rgb.r = tex2D(bitmap, uv + float2(cos(rgbAngle.r), sin(rgbAngle.r)) * radius).r;
				rgb.g = tex2D(bitmap, uv + float2(cos(rgbAngle.g), sin(rgbAngle.g)) * radius).g;
				rgb.b = tex2D(bitmap, uv + float2(cos(rgbAngle.b), sin(rgbAngle.b)) * radius).b;
				return rgb;
			}	

			// https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83
			float rand (float2 n)
			{
			  return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
			}

			fixed4 frag (v2f_img i) : SV_Target 
			{
				float2 uv = i.uv;
				uv = lerp(uv, floor(uv * _Resolution) / _Resolution, step(0.75, _LightRatio));

				float t = _Time;

				float scalineOffset = (rand(uv.yy) * 2. - 1.) * (0.01 + rand(uv.yy + float2(t, 0.)) * 0.02);

				uv = lerp(uv, uv + float2(scalineOffset, 0.), _HurtRatio);

				// fixed4 color = tex2D(_MotionTexture, uv);
				fixed4 color = fixed4(0,0,0,1);
				color.rgb = lerp(tex2D(_MotionTexture, uv).rgb, rgbOffset(_MotionTexture, uv, 3. / _Resolution.y, 0.), _HurtRatio);

				fixed4 webcam = tex2D(_WebcamTexture, uv);

				color.rgb = lerp(color.rgb, min(1, color.rgb + webcam.rgb * 0.5), _ShowWebcam);
				color.rgb *= _LightRatio;

				// fixed4 gui = tex2D(_GUITexture, uv);
				//// color.rgb = lerp(color.rgb, min(1, color.rgb + gui.rgb), 1. - step(0.75, _LightRatio));
				// color.rgb = min(1, color.rgb + gui.rgb * gui.a);

				// fixed3 silhouette = step(0.5, Luminance(edgeFilter(_WebcamTexture, uv, _Resolution)));
				fixed3 silhouette = Luminance(edgeFilter(_WebcamTexture, uv, _Resolution));
				// color.rgb = min(1, color.rgb + silhouette * 0.5);
				// color.rgb = lerp(color.rgb, webcam, step(0.5, silhouette));
				color.rgb *= 1 - clamp(silhouette - 0.25, 0, 1);

				// fixed4 game = tex2D(_GameTexture, uv);
				// color.rgb += min(1, color.rgb + game.rgb * game.a);

				color.rgb = lerp(color.rgb, 1. - color.rgb, _InvertColor);

				return color;
			}
			ENDCG
		}
	}
}
