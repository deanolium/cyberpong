Shader "Hidden/CRTShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BleedDistance ("Bleed Distance", float) = 0.01
		_BleedWeightRed ("Bleed Weight Red", float) = 0.6
		_BleedWeightBlue ("Bleed Weight Blue", float) = 0.6
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag			
			#include "UnityCG.cginc"
			#pragma target 3.0
			#define PI 3.141592653589

			uniform sampler2D _MainTex;
			uniform float2 _InputSize;
			uniform float2 _OutputSize;
			uniform float2 _TextureSize;
			uniform float2 _Texcoord;
			uniform float2 _One;
			uniform float _Distortion;		
			uniform float _Factor;
			uniform float _BleedDistance;
			uniform float _BleedWeightRed;
			uniform float _BleedWeightBlue;

			float2 RadialDistortion(float2 coord) 
			{
				coord *= _TextureSize / _InputSize;
				float2 cc = coord - 0.5f;
				float dist = dot(cc,cc) * _Distortion;
				return (coord + cc * (1.0f + dist) * dist) * _InputSize / _TextureSize;
			}

		
			float4 frag (v2f_img i) : COLOR
			{
				_One = 1.0f / _TextureSize;
				_Texcoord = i.uv;
				_InputSize = _TextureSize;
				_OutputSize = _TextureSize;
				_Factor = _Texcoord.x * _TextureSize.x * _OutputSize.x / _InputSize.x;

				float2 xy = RadialDistortion(_Texcoord);

				float2 ratio = xy * _TextureSize - float2(0.5f, 0.5f);
				float2 uvratio = frac(ratio);

				xy.y = (floor(ratio.y) + 0.5f) / _TextureSize;

				float4 col = tex2D(_MainTex, xy);
				float4 col_redbleed = tex2D(_MainTex, float2(xy.x - _BleedDistance, xy.y));
				float4 col_bluebleed = tex2D(_MainTex, float2(xy.x + _BleedDistance, xy.y));

				float3 res = col.rgb + _BleedWeightRed * float3(col_redbleed.g, 0.0f, 0.0f) + _BleedWeightBlue * float3(0.0f, 0.0f, col_bluebleed.r); 

				float3 rgb1 = float3(1.0f, 0.9f, 1.0f);
				float3 rgb2 = float3(0.9f, 1.0f, 0.9f);

				float3 dotMaskWeights = lerp(rgb1, rgb2, floor(fmod(_Factor, 1.6f)));
				res *= dotMaskWeights;
				
				// just invert the colors
				return float4(res, 1.0f);
			}
			ENDCG
		}
	}
}
