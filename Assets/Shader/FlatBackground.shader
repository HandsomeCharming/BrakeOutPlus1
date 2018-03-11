Shader "Unlit/FlatBackground"
{
	Properties
	{
		_ColorUpA("Up Color A", Color) = (1,0.7,0.7,1)
		_ColorDownA("Down Color A", Color) = (0.7, 1, 0.7, 1)
		_ColorUpB("Up Color B", Color) = (1,0.7,0.7,1)
		_ColorDownB("Down Color B", Color) = (0.7, 1, 0.7, 1)
		_ColorLerp("Lerp factor", float) = 0.0
		_ColorBound("Color Bound", float) = 0.1

		_ScreenSizeX("Screen Size X", float) = 800
		_ScreenSizeY("Screen Size Y", float) = 600
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD0;
			};

			fixed4 _ColorUpA;
			fixed4 _ColorDownA;
			fixed4 _ColorUpB;
			fixed4 _ColorDownB;
			float _ColorLerp;
			float _ScreenSizeX;
			float _ScreenSizeY;
			float _ColorBound;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float2 screenUV = i.screenPos.xy / i.screenPos.w;
				float lerpFactor = screenUV.y;//(screenUV.y+1.0f)/2.0f;
				lerpFactor -= _ColorBound;
				lerpFactor /= (1.0f - 2.0f * _ColorBound);
				//lerpFactor = clamp(lerpFactor, 0.0f, 1.0f);

				fixed4 colorDown = lerp(_ColorDownA, _ColorDownB, _ColorLerp);
				fixed4 colorUp = lerp(_ColorUpA, _ColorUpB, _ColorLerp);

				fixed4 col = lerp(colorDown, colorUp, lerpFactor);
				return col;
			}
			ENDCG
		}
	}
}
