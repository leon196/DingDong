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
			#pragma target 3.0
			#include "UnityCG.cginc"
			#define PI 3.1415926535897
			
			sampler2D _MainTex;
			sampler2D _MotionTexture;
			sampler2D _WebcamTexture;
			sampler2D _WebcamTextureLast;
			sampler2D _GUITexture;
			sampler2D _BonusTexture;
			sampler2D _MalusTexture;

			float _TresholdMotion;
			float _FadeOutRatio;
			float _SplashesRatio;
			float2 _CollisionPosition;
			float2 _Resolution;
			float _BonusSize;
			float2 _BonusPosition;
			float4 _BonusColor;
			float4 _GUIColor;

			// http://stackoverflow.com/questions/12964279/whats-the-origin-of-this-glsl-rand-one-liner
			float rand (float2 co) {
			  return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
			}

			fixed4 frag (v2f_img i) : SV_Target 
			{
				fixed4 col = fixed4(1,1,1,1);
				float2 uv = i.uv;
				float t = _Time;
				
				// Bonus circle
				float2 position = _BonusPosition - i.uv;
				position.x *= _Resolution.x / _Resolution.y;
				float circle = step(length(position), _BonusSize);
				position = (position / (_BonusSize * 1.5) + float2(0.5, 0.5));
				float4 circleTexture = tex2D(_BonusTexture, position);
				// col.rgb = lerp(col.rgb, circleTexture.rgb * _BonusColor, circleTexture.a * circle);

				// Splash origin vector
				position = uv - _CollisionPosition;
				float angle = atan2(position.y, position.x);// + t;
				float variation = rand(float2(floor(angle * 128.) / 128., 0.));
				uv -= float2(cos(angle), sin(angle)) * 0.01 * _SplashesRatio * variation;

				// Brush vector
				angle = rand(position) * PI * 2.;
				uv += float2(cos(angle), sin(angle)) * 0.002 * _SplashesRatio;

				// River vector
				angle = Luminance(tex2D(_WebcamTexture, i.uv)) * PI * 2.;
				uv += float2(cos(angle), sin(angle)) * 0.002 * _SplashesRatio;

				// Cheap Motion Detection
				float4 motion = fixed4(1,1,1,1);
				float3 current = tex2D(_WebcamTexture, uv).rgb;
				float3 last = tex2D(_WebcamTextureLast, uv).rgb;
				motion.rgb *= step(_TresholdMotion, abs(current.r - last.r) + abs(current.g - last.g) + abs(current.b - last.b));
				// motion.rgb = lerp(motion.rgb, current, _SplashesRatio);

				float4 fadeOut = fixed4(1,1,1,1);

				// Add Bonus
				fadeOut.rgb = lerp(tex2D(_MotionTexture, uv).rgb, _BonusColor, circle);// * (1 - circleTexture.a));

				// Ghost effect
				fadeOut.rgb *= lerp(_FadeOutRatio, 0.98, _SplashesRatio);

				// Layer Motion
				col.rgb = lerp(fadeOut, motion, step(0.5, motion));


				// Layer GUI
				// fixed4 gui = tex2D(_GUITexture, i.uv);
				// col.rgb = lerp(col.rgb, _GUIColor, gui.a);

				return col;
			}
			ENDCG
		}
	}
}
