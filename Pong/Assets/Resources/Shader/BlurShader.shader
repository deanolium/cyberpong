Shader "Custom/BlurShader" {
	Properties {
		_Size ("Size of Blur", Range(0,20)) = 1.5
	}
	SubShader {
		Tags { "RenderType"="Transparent" }

		GrabPass {
			"_BackgroundTexture"
		}

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag 
			#include "UnityCG.cginc"

			struct v2f {
				float4 uvGrab: TEXCOORD0;
				float4 pos: POSITION;
			};

			v2f vert(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				o.uvGrab.xy = (float2(o.pos.x, o.pos.y * scale) + o.pos.w) * 0.5;
				o.uvGrab.zw = o.pos.zw;
				return o;
			}

			sampler2D _BackgroundTexture;
			float4 _BackgroundTexture_TexelSize;
			float _Size;

			half4 frag(v2f i) : COLOR {				
				half4 bgcolor = half4(0,0,0,0); //tex2Dproj(_BackgroundTexture, i.uvGrab);
				#define GRABPIXEL(weight, kernelX, kernelY) tex2Dproj(_BackgroundTexture, UNITY_PROJ_COORD(float4(i.uvGrab.x + _BackgroundTexture_TexelSize.x * kernelX * _Size, i.uvGrab.y + _BackgroundTexture_TexelSize.y * kernelY * _Size, i.uvGrab.z, i.uvGrab.w))) * weight

				bgcolor += GRABPIXEL(0.18, 0, -4);
				bgcolor += GRABPIXEL(0.15, 1, -3);
				bgcolor += GRABPIXEL(0.09, 2, -2);
				bgcolor += GRABPIXEL(0.01, 3, -1);
				bgcolor += GRABPIXEL(0.14, 0, 0);
				bgcolor += GRABPIXEL(0.01, 3, +1);
				bgcolor += GRABPIXEL(0.09, 2, +2);
				bgcolor += GRABPIXEL(0.15, 1, +3);
				bgcolor += GRABPIXEL(0.18, 0, +4);

				bgcolor += GRABPIXEL(0.18, -4, 0);
				bgcolor += GRABPIXEL(0.15, -3, -1);
				bgcolor += GRABPIXEL(0.09, -2, -2);
				bgcolor += GRABPIXEL(0.01, -1, -3);
				bgcolor += GRABPIXEL(0.14, 0, 0);
				bgcolor += GRABPIXEL(0.01, +1, -3);
				bgcolor += GRABPIXEL(0.09, +2, -2);
				bgcolor += GRABPIXEL(0.15, +3, -1);
				bgcolor += GRABPIXEL(0.18, +4, 0);
				
				bgcolor += GRABPIXEL(0.18, 0, -4);
				bgcolor += GRABPIXEL(0.15, -1, -3);
				bgcolor += GRABPIXEL(0.09, -2, -2);
				bgcolor += GRABPIXEL(0.01, -3, -1);
				bgcolor += GRABPIXEL(0.14, 0, 0);
				bgcolor += GRABPIXEL(0.01, -3, +1);
				bgcolor += GRABPIXEL(0.09, -2, +2);
				bgcolor += GRABPIXEL(0.15, -1, +3);
				bgcolor += GRABPIXEL(0.18, 0, +4);

				bgcolor += GRABPIXEL(0.18, -4, 0);
				bgcolor += GRABPIXEL(0.15, -3, 1);
				bgcolor += GRABPIXEL(0.09, -2, 2);
				bgcolor += GRABPIXEL(0.01, -1, 3);
				bgcolor += GRABPIXEL(0.14, 0, 0);
				bgcolor += GRABPIXEL(0.01, +1, 3);
				bgcolor += GRABPIXEL(0.09, +2, 2);
				bgcolor += GRABPIXEL(0.15, +3, 1);
				bgcolor += GRABPIXEL(0.18, +4, 0);

				bgcolor = bgcolor * 0.25;

				return bgcolor;
			}

			ENDCG
		}
	}
}
