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
			sampler2D _GameTexture;
			
			float4 _GUIColor;

			float2 _SplashPosition;
			float2 _Resolution;

			float _TresholdMotion;
			float _FadeOutRatio;
			float _SplashRatio;

			// http://stackoverflow.com/questions/12964279/whats-the-origin-of-this-glsl-rand-one-liner
			float rand (float2 co) {
			  return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
			}

			fixed4 frag (v2f_img i) : SV_Target 
			{
				fixed4 col = fixed4(1,1,1,1);
				float2 uv = i.uv;
				float t = _Time;

				// Splash
				float2 position = uv - _SplashPosition;
				float angle = atan2(position.y, position.x);// + t;
				float variation = rand(float2(floor(angle * 128.) / 128., 0.));
				uv -= float2(cos(angle), sin(angle)) * 0.01 * _SplashRatio * variation;
				
				angle = rand(position) * PI * 2.;
				uv += float2(cos(angle), sin(angle)) * 0.002 * _SplashRatio;

				angle = Luminance(tex2D(_WebcamTexture, i.uv)) * PI * 2.;
				uv += float2(cos(angle), sin(angle)) * 0.002 * _SplashRatio;

				// Cheap Motion Detection
				float4 motion = fixed4(1,1,1,1);
				float3 current = tex2D(_WebcamTexture, uv).rgb;
				float3 last = tex2D(_WebcamTextureLast, uv).rgb;
				motion.rgb *= step(_TresholdMotion, abs(current.r - last.r) + abs(current.g - last.g) + abs(current.b - last.b));

				// Ghost effect
				float ratio = lerp(_FadeOutRatio, 0.98, _SplashRatio);
				// float4 existingFrag = tex2D(_MotionTexture, uv+0.01.xx);
				float4 existingFrag = tex2D(_MotionTexture, uv+float2(cos(angle), sin(angle)) * 0.01);
				float3 fragNormal = normalize(existingFrag.xyz);
				float3 fadeOutXYZ = lerp (fragNormal*0.2, existingFrag.xyz, ratio);
				float4 fadeOut = float4(fadeOutXYZ, existingFrag.w); // pack it to float 4
				//float4 fadeOut = existingFrag * ratio; // old

				float4 newColour = motion*normalize(fadeOut)*2;
				// Layer Motion
				col.rgb = lerp(fadeOut, newColour, step(0.5, motion));

				// col.rgb = _GUIColor * Luminance(col.rgb);

				// Game collectible
				fixed4 game = tex2D(_GameTexture, uv);
				col.rgb = lerp(col.rgb, game.rgb, game.a);


				// GUI
				fixed4 gui = tex2D(_GUITexture, uv);
				col.rgb = lerp(col.rgb, gui.rgb * _GUIColor.rgb * 0.99, gui.a * _GUIColor.a);

				return col;
			}
			ENDCG
		}
	}
}
