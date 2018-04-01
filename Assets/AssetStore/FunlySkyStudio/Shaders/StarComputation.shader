// Sky Studio
// Author: Jason Ederle

Shader "Hidden/Funly/Sky Studio/Computation/Stars"
{
	Properties
	{
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
			#pragma shader_feature LINEAR
      #pragma shader_feature EASE_IN_OUT

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
      
      float _StarDensity;
      float _ImageWidth;
      float _ImageHeight;
      int _NumStarPoints;
      float4 _RandomSeed;

      #define _PI 3.14159265358
      #define _PI_2 (_PI / 2)
      #define _2_PI (_PI * 2)

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}
         
      float rand(float2 co){
        return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
      }

      float rangWithNeg(float2 co) {
        float valueForNeg = rand(co * 1.234123f);
        float valueSign = frac(valueForNeg) < .5 ? -1.0f : 1.0f;
        return rand(co) * valueSign;
      }

      float4 GenerateNextStarPoint(int i, float2 uv) {
        float xRand = rangWithNeg(float2(uv.x + i, i + frac(uv.y)));
        float yRand = rangWithNeg(float2(uv.x + xRand, uv.y + xRand));
        float zRand = rangWithNeg(float2(uv.x + yRand, uv.y + xRand));
        float noise = frac(abs(rand(uv * i)));

        return float4(normalize(float3(xRand, yRand, zRand)), noise);
      }

      float3 RadiusAtHeight(float yPosition) {
        return abs(cos(asin(yPosition)));
      }

      float3 SphericalCoordToPoint(float yPosition, float radAngle) {
        float radius = RadiusAtHeight(yPosition);
      }

      float3 GetSpherePointForUV(float2 uv) {
        float xAngle = uv.x * 6.2831853f;
        float yPosition = lerp(-1.0f, 1.0f, uv.y);
        float radius = RadiusAtHeight(yPosition);

        return normalize(float3(
          radius * cos(xAngle),
          yPosition,
          radius * sin(xAngle)
        ));
      }

      float4 GetClosestStarPoint(float2 uv) {
        float4 closestStarPoint = float4(100, 100, 100, 0);
        float3 fragPoint = GetSpherePointForUV(uv);
        float shortestDistance = 100.0f;

        for (int i = 0; i < _NumStarPoints; i++) {
          float4 randomStarPoint = GenerateNextStarPoint(i + 1, _RandomSeed.xyz);
          float currentStarDistance = distance(randomStarPoint.xyz, fragPoint);
          if (currentStarDistance <= shortestDistance) {
            closestStarPoint = randomStarPoint;
            shortestDistance = currentStarDistance;
          }
        }

        return closestStarPoint;
      }

      float4 RenderNearbyStarImage(float2 uv) {
        float2 areaUV = float2(
          uv.x * 2.0f,
          (uv.y - .5f) * 2);

        return GetClosestStarPoint(areaUV);
      }

      float Atan2Positive(float y, float x) {
        float angle = atan2(y, x);
        
        // This is the same as: angle = (angle > 0) ? angle : _PI + (_PI + angle)
        float isPositive = step(0, angle);
        float posAngle = angle * isPositive;
        float negAngle = (_PI + (_PI + angle)) * !isPositive;

        return posAngle + negAngle;
      }

      float AngleToReachTarget(float2 spot, float targetAngle) {
        float angle = Atan2Positive(spot.y, spot.x);
        return (_2_PI - angle) + targetAngle;
      }

      inline float2 Rotate2d(float2 p, float angle) {
        return mul(float2x2(cos(angle), -sin(angle),
                            sin(angle), cos(angle)),
                   p);
      }

      float3 RotateAroundYAxis(float3 p, float angle) {
        float2 rotation = Rotate2d(p.xz, angle);
        return float3(rotation.x, p.y, rotation.y);
      }

      float2 CalculateStarRotation(float3 starPoint) {
        float3 starPos = float3(starPoint);

        float yRotationAngle = AngleToReachTarget(
          float2(starPos.x, starPos.z), _PI_2);

        starPos = RotateAroundYAxis(starPos, yRotationAngle);

        float xRotationAngle = AngleToReachTarget(
          float2(starPos.z, starPos.y), 0.0f);

        return float2(xRotationAngle, yRotationAngle);
      }

      float2 GetStarPointRotation(float2 uv) {
        float3 starPoint = GetClosestStarPoint(uv);        
        return CalculateStarRotation(starPoint);
      }

      float4 RenderStarRotationImage(float2 uv) {
        float2 areaUV = float2(
          (uv.x - .5f) * 2.0f,
          (uv.y - .5f) * 2.0f);
      
        float2 starRot = GetStarPointRotation(areaUV);

        return float4(starRot.x, starRot.y, 0, 1);
      }

			float4 frag (v2f i) : SV_Target
			{
        // Bottom is currently unused.
        if (i.uv.y < .5f) {
          return float4(0, 0, 0, 0);
        }
        
        float4 col;
        if (i.uv.x <= .5f) {
          col = RenderNearbyStarImage(i.uv);
        } else {
          col = RenderStarRotationImage(i.uv);
        }
        
				return col;
			}
			ENDCG
		}
	}
}
