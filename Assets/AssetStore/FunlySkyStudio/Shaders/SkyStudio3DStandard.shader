// Copyright(c) 2017 Funly LLC
//
// Author: Jason Ederle
// Description: Renders a customizable sky for a 3D skybox sphere.
// Contact: jason@funly.io

Shader "Funly/Sky Studio/Skybox/3D Standard" {
  Properties {
    // Gradient Sky.
    _GradientSkyUpperColor("Sky Top Color", Color) = (.47, .45, .75, 1)            // Color of sky.
    _GradientSkyMiddleColor("Sky Middle Color", Color) = (1, 1, 1, 1)              // Color in the middle of sky 3 way gradient.
    _GradientSkyLowerColor("Sky Lower Color", Color) = (.7, .53, .69, 1)           // Color of horizon.
    _GradientFadeBegin("Horizon Fade Begin", Range(-1, 1)) = -.179                 // Position to begin horizon fade into sky.
    _GradientFadeEnd("Horizon Fade End", Range(-1, 1)) = .302                      // Position to end horizon fade into sky.
    _GradientFadeMiddlePosition("Horizon Fade Middle Position", Range(0, 1)) = .5  // Position of the middle gradient color.

    // Shrink stars closer to horizon.
    _HorizonScaleFactor("Star Horizon Scale Factor", Range(0, 1)) = .7

    // Cubemap background.
    [NoScaleOffset]_MainTex("Background Cubemap", CUBE) = "white" {}      // Cubemap for custom background behind stars.

    // Star fading.
    _StarFadeBegin("Star Fade Begin", Range(-1, 1)) = .067                // Height to begin star fade in.
    _StarFadeEnd("Star Fade End", Range(-1, 1)) = .36                     // Height where all stars are faded in at.

    // Star Layer 1.
    [NoScaleOffset]_StarLayer1Tex("Star 1 Texture", 2D) = "white" {}
    _StarLayer1Color("Star Layer 1 - Color", Color) = (1, 1, 1, 1)                              // Color tint for stars.
    _StarLayer1Density("Star Layer 1 - Star Density", Range(0, .05)) = .01                      // Space between stars.
    _StarLayer1MaxRadius("Star Layer 1 - Star Size", Range(0, .1)) = .007                       // Max radius of stars.
    _StarLayer1TwinkleAmount("Star Layer 1 - Twinkle Amount", Range(0, 1)) = .775               // Percent of star twinkle amount.
    _StarLayer1TwinkleSpeed("Star Layer 1 - Twinkle Speed", float) = 2.0                        // Twinkle speed.
    _StarLayer1RotationSpeed("Star Layer 1 - Rotation Speed", float) = 2                        // Rotation speed of stars.
    _StarLayer1EdgeFade("Star Layer 1 - Edge Feathering", Range(0.0001, .9999)) = .2            // Softness of star blending with background.
    _StarLayer1HDRBoost("Star Layer 1 - HDR Bloom Boost", Range(1, 10)) = 1.0                   // Boost star colors so they glow with bloom filters.
    _StarLayer1SpriteDimensions("Star Layer 1 Sprite Dimensions", Vector) = (0, 0, 0, 0)        // Dimensions of columns (x), and rows (y) in sprite sheet.
    _StarLayer1SpriteItemCount("Star Layer 1 Sprite Total Items", int) = 1                      // Total number of items in sprite sheet.
    _StarLayer1SpriteAnimationSpeed("Star Layer 1 Sprite Speed", int) = 1                       // Speed of the sprite sheet animation.
    [NoScaleOffset]_StarLayer1DataTex("Star Layer 1 - Data Image", 2D) = "black" {}           // Data image with star positions.
    
    // Star Layer 2. - See property descriptions from star layer 1.
    [NoScaleOffset]_StarLayer2Tex("Star 2 Texture", 2D) = "white" {}
    _StarLayer2Color("Star Layer 2 - Color", Color) = (1, .5, .96, 1)
    _StarLayer2Density("Star Layer 2 - Star Density", Range(0, .05)) = .01
    _StarLayer2MaxRadius("Star Layer 2 - Star Size", Range(0, .4)) = .014
    _StarLayer2TwinkleAmount("Star Layer 2 - Twinkle Amount", Range(0, 1)) = .875
    _StarLayer2TwinkleSpeed("Star Layer 2 - Twinkle Speed", float) = 3.0
    _StarLayer2RotationSpeed("Star Layer 2 - Rotation Speed", float) = 2
    _StarLayer2EdgeFade("Star Layer 2 - Edge Feathering", Range(0.0001, .9999)) = .2
    _StarLayer2HDRBoost("Star Layer 2 - HDR Bloom Boost", Range(1, 10)) = 1.0
    _StarLayer2SpriteDimensions("Star Layer 2 Sprite Dimensions", Vector) = (0, 0, 0, 0)
    _StarLayer2SpriteItemCount("Star Layer 2 Sprite Total Items", int) = 1
    _StarLayer2SpriteAnimationSpeed("Star Layer 2 Sprite Speed", int) = 1
    [NoScaleOffset]_StarLayer2DataTex("Star Layer 2 - Data Image", 2D) = "black" {}

    // Star Layer 3. - See property descriptions from star layer 1.
    [NoScaleOffset]_StarLayer3Tex("Star 3 Texture", 2D) = "white" {}
    _StarLayer3Color("Star Layer 3 - Color", Color) = (.22, 1, .55, 1)
    _StarLayer3Density("Star Layer 3 - Star Density", Range(0, .05)) = .01
    _StarLayer3MaxRadius("Star Layer 3 - Star Size", Range(0, .4)) = .01
    _StarLayer3TwinkleAmount("Star Layer 3 - Twinkle Amount", Range(0, 1)) = .7
    _StarLayer3TwinkleSpeed("Star Layer 3 - Twinkle Speed", float) = 1.0
    _StarLayer3RotationSpeed("Star Layer 3 - Rotation Speed", float) = 2
    _StarLayer3EdgeFade("Star Layer 3 - Edge Feathering", Range(0.0001, .9999)) = .2
    _StarLayer3HDRBoost("Star Layer 3 - HDR Bloom Boost", Range(1, 10)) = 1.0
    _StarLayer3SpriteDimensions("Star Layer 3 Sprite Dimensions", Vector) = (0, 0, 0, 0)
    _StarLayer3SpriteItemCount("Star Layer 3 Sprite Total Items", int) = 1
    _StarLayer3SpriteAnimationSpeed("Star Layer 3 Sprite Speed", int) = 1
    [NoScaleOffset]_StarLayer3DataTex("Star Layer 1 - Data Image", 2D) = "black" {}

    // Moon properties.
    [NoScaleOffset]_MoonTex("Moon Texture", 2D) = "white" {}               // Moon image.
    _MoonColor("Moon Color", Color) = (.66, .65, .55, 1)                   // Moon tint color.
    _MoonRadius("Moon Size", Range(0, 1)) = .1                             // Radius of the moon.
    _MoonEdgeFade("Moon Edge Feathering", Range(0.0001, .9999)) = .3       // Soften edges of moon texture.
    _MoonHDRBoost("Moon HDR Bloom Boost", Range(1, 10)) = 1                // Control brightness for HDR bloom filter.
    _MoonSpriteDimensions("Moon Sprite Dimensions", Vector) = (0, 0, 0, 0) // Dimensions of columns (x), and rows (y) in sprite sheet.
    _MoonSpriteItemCount("Moon Sprite Total Items", int) = 1               // Total number of items in sprite sheet.
    _MoonSpriteAnimationSpeed("Moon Sprite Speed", int) = 1                // Speed of the sprite sheet animation.
    _MoonComputedPositionData("Moon Position Data" , Vector) = (0, 0, 0, 0)  // Precomputed position data.
    _MoonComputedRotationData("Moon Rotation Data", Vector) = (0, 0, 0, 0)   // Precomputed rotation data.

    // Sun properties.
    [NoScaleOffset]_SunTex("Sun Texture", 2D) = "white" {}               // Sun image.
    _SunColor("Sun Color", Color) = (.66, .65, .55, 1)                   // Sun tint color.
    _SunRadius("Sun Size", Range(0, 1)) = .1                             // Radius of the Sun.
    _SunEdgeFade("Sun Edge Feathering", Range(0.0001, .9999)) = .3       // Soften edges of Sun texture.
    _SunHDRBoost("Sun HDR Bloom Boost", Range(1, 10)) = 1                // Control brightness for HDR bloom filter.
    _SunSpriteDimensions("Sun Sprite Dimensions", Vector) = (0, 0, 0, 0) // Dimensions of columns (x), and rows (y) in sprite sheet.
    _SunSpriteItemCount("Sun Sprite Total Items", int) = 1               // Total number of items in sprite sheet.
    _SunSpriteAnimationSpeed("Sun Sprite Speed", int) = 1                // Speed of the sprite sheet animation.
    _SunComputedPositionData("Sun Position Data" , Vector) = (0, 0, 0, 0)  // Precomputed position data.
    _SunComputedRotationData("Sun Rotation Data", Vector) = (0, 0, 0, 0)   // Precomputed rotation data.

    // Cloud properties.
    [NoScaleOffset]_CloudNoiseTexture("Cloud Texture", 2D) = "white" {}         // Cloud texture.
    _CloudDensity("Cloud Density", Range(0, 1)) = .25                           // Cloud density.
    _CloudSpeed("Cloud Speed", Range(0, 1)) = .1                                // Cloud speed.
    _CloudDirection("Cloud Direction", Range(0, 6.283)) = 1.0                   // Cloud direction.
    _CloudHeight("Cloud Height", Range(0, 1)) = .5                              // Cloud height.
    _CloudColor1("Cloud 1 Color", Color) = (1, 1, 1, 1)                         // Cloud color 1.
    _CloudColor2("Cloud 2 Color", Color) = (.6, .6, .6, 1)                      // Cloud color 2.

    _CloudFadePosition("Cloud Fade Position", Range(0, .97)) = .74    // Position that the clouds will begin fading out at.
    _CloudFadeAmount("Cloud Fade Amount", Range(0, 1)) = .5           // Amount of fade to clouds.

    _HorizonFogColor("Fog Color", Color) = (1, 1, 1, 1)               // Fog color.
    _HorizonFogDensity("Fog Density", Range(0, 1)) = .12              // Density and visibility of the fog.
    _HorizonFogLength("Fog Height", Range(.03, 1)) = .1               // Height the fog reaches up into the skybox.
  }

  SubShader {
    Tags { "RenderType"="Opaque" "Queue"="Background" "IgnoreProjector"="true" "PreviewType" = "Skybox" }
    LOD 100
    ZWrite Off

    Pass {
      CGPROGRAM
      #pragma target 3.0
      #pragma multi_compile_fog

      #pragma shader_feature GRADIENT_BACKGROUND
      #pragma shader_feature STAR_LAYER_1
      #pragma shader_feature STAR_LAYER_2
      #pragma shader_feature STAR_LAYER_3
      #pragma shader_feature STAR_LAYER_1_CUSTOM_TEXTURE
      #pragma shader_feature STAR_LAYER_2_CUSTOM_TEXTURE
      #pragma shader_feature STAR_LAYER_3_CUSTOM_TEXTURE
      #pragma shader_feature STAR_LAYER_1_SPRITE_SHEET
      #pragma shader_feature STAR_LAYER_2_SPRITE_SHEET
      #pragma shader_feature STAR_LAYER_3_SPRITE_SHEET
      #pragma shader_feature MOON
      #pragma shader_feature MOON_CUSTOM_TEXTURE
      #pragma shader_feature MOON_SPRITE_SHEET
      #pragma shader_feature SUN
      #pragma shader_feature SUN_CUSTOM_TEXTURE
      #pragma shader_feature SUN_SPRITE_SHEET
      #pragma shader_feature HORIZON_FOG
      #pragma shader_feature GLOBAL_FOG
      #pragma shader_feature CLOUDS
      #pragma shader_feature SUN_ALPHA_BLEND
      #pragma shader_feature SUN_ROTATION
      #pragma shader_feature MOON_ALPHA_BLEND
      #pragma shader_feature MOON_ROTATION

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
      };

      struct v2f {
        float verticalPosition : TEXCOORD0;
#ifdef HORIZON_FOG
  #ifdef GLOBAL_FOG
        UNITY_FOG_COORDS(1)
  #endif
#endif
        float4 vertex : SV_POSITION;
        float3 smoothVertex : TEXCOORD2;

#ifdef CLOUDS
        float4 cloudUVs : TEXCOORD3;
#endif
      };

      // Cubemap.
      samplerCUBE _MainTex;
      float4 _MainTex_ST;

      // Gradient sky.
      float _UseGradientSky;
      float4 _GradientSkyUpperColor;
      float4 _GradientSkyMiddleColor;
      float4 _GradientSkyLowerColor;
      float _GradientFadeMiddlePosition;

      float _GradientFadeBegin;
      float _GradientFadeEnd;

      float _StarFadeBegin;
      float _StarFadeEnd;

#ifdef STAR_LAYER_1
      // Star Layer 1
      sampler2D _StarLayer1Tex;
      float4 _StarLayer1Tex_ST;
      float4 _StarLayer1Color;
      float _StarLayer1MaxRadius;
      float _StarLayer1Density;
      float _StarLayer1TwinkleAmount;
      float _StarLayer1TwinkleSpeed;
      float _StarLayer1RotationSpeed;
      float _StarLayer1EdgeFade;
      sampler2D _StarLayer1DataTex;
      float4 _StarLayer1DataTex_ST;;
      float _StarLayer1HDRBoost;
  #ifdef STAR_LAYER_1_CUSTOM_TEXTURE
    #ifdef STAR_LAYER_1_SPRITE_SHEET
      float2 _StarLayer1SpriteDimensions;
      int _StarLayer1SpriteItemCount;
      int _StarLayer1SpriteAnimationSpeed;
    #endif
  #endif
#endif

#ifdef STAR_LAYER_2
      // Star Layer 2
      sampler2D _StarLayer2Tex;
      float4 _StarLayer2Tex_ST;
      float4 _StarLayer2Color;
      float _StarLayer2MaxRadius;
      float _StarLayer2Density;
      float _StarLayer2TwinkleAmount;
      float _StarLayer2TwinkleSpeed;
      float _StarLayer2RotationSpeed;
      float _StarLayer2EdgeFade;
      sampler2D _StarLayer2DataTex;
      float4 _StarLayer2DataTex_ST;;
      float _StarLayer2HDRBoost;
  #ifdef STAR_LAYER_2_CUSTOM_TEXTURE
    #ifdef STAR_LAYER_2_SPRITE_SHEET
      float2 _StarLayer2SpriteDimensions;
      int _StarLayer2SpriteItemCount;
      int _StarLayer2SpriteAnimationSpeed;
    #endif
  #endif
#endif

#ifdef STAR_LAYER_3
      // Star Layer 3
      sampler2D _StarLayer3Tex;
      float4 _StarLayer3Tex_ST;
      float4 _StarLayer3Color;
      float _StarLayer3MaxRadius;
      float _StarLayer3Density;
      float _StarLayer3TwinkleAmount;
      float _StarLayer3TwinkleSpeed;
      float _StarLayer3RotationSpeed;
      float _StarLayer3EdgeFade;
      sampler2D _StarLayer3DataTex;
      float4 _StarLayer3DataTex_ST;;
      float _StarLayer3HDRBoost;
  #ifdef STAR_LAYER_3_CUSTOM_TEXTURE
    #ifdef STAR_LAYER_3_SPRITE_SHEET
      float2 _StarLayer3SpriteDimensions;
      int _StarLayer3SpriteItemCount;
      int _StarLayer3SpriteAnimationSpeed;
    #endif
  #endif
#endif

      float _HorizonScaleFactor;

#ifdef MOON
      // Moon
  #ifdef MOON_CUSTOM_TEXTURE
      sampler2D _MoonTex;
      float _MoonRotationSpeed;
    #ifdef MOON_SPRITE_SHEET
      float2 _MoonSpriteDimensions;
      int _MoonSpriteItemCount;
      int _MoonSpriteAnimationSpeed;
    #endif
  #endif
      float4 _MoonColor;
      float _MoonRadius;
      float _MoonEdgeFade;
      float _MoonHDRBoost;
      float4 _MoonComputedPositionData;
      float4 _MoonComputedRotationData;
#endif

#ifdef SUN
      // Sun
  #ifdef SUN_CUSTOM_TEXTURE
      sampler2D _SunTex;
      float _SunRotationSpeed;
    #ifdef SUN_SPRITE_SHEET
      float2 _SunSpriteDimensions;
      int _SunSpriteItemCount;
      int _SunSpriteAnimationSpeed;
    #endif
  #endif
      float4 _SunColor;
      float _SunRadius;
      float _SunEdgeFade;
      float _SunHDRBoost;
      float4 _SunComputedPositionData;
      float4 _SunComputedRotationData;
#endif

#ifdef CLOUDS
      // Clouds
      sampler2D _CloudNoiseTexture;
			float _CloudDensity;
      float _CloudSpeed;
      float _CloudDirection;
      float _CloudHeight;
			float4 _CloudColor1;
      float4 _CloudColor2;
      float _CloudFadePosition;
      float _CloudFadeAmount;
#endif

#if HORIZON_FOG
      float4 _HorizonFogColor;
      float _HorizonFogDensity;
      float _HorizonFogLength;
#endif

      #define _PI 3.14159265358
      #define _PI_2 (_PI / 2)
      #define _2_PI (_PI * 2)
      #define _MAX_CLOUD_COVERAGE 7
      #define _CLOUD_HEIGHT_LIMITS float2(30, 100)

      // Returns color for this fragment using both the background and star color.
      half4 MergeStarIntoBackground(float4 background, half4 starColor) {
        half starFadeAmount = starColor.a;

        background.a = 1;
        starColor.a = 1;

        // Additive overlap with star scaled using alpha.
        return background + (starColor * starFadeAmount);
      }
      
      inline float2 Rotate2d(float2 p, float angle) {
        return mul(float2x2(cos(angle), -sin(angle),
                            sin(angle), cos(angle)),
                   p);
      }

      float Atan2Positive(float y, float x) {
        float angle = atan2(y, x);
        
        // This is the same as: angle = (angle > 0) ? angle : _PI + (_PI + angle)
        float isPositive = step(0, angle);
        float posAngle = angle * isPositive;
        float negAngle = (_PI + (_PI + angle)) * !isPositive;

        return posAngle + negAngle;
      }

      float3 RotateAroundXAxis(float3 p, float angle) {
        float2 rotation = Rotate2d(p.zy, angle);
        return float3(p.x, rotation.y, rotation.x);
      }

      float3 RotateAroundYAxis(float3 p, float angle) {
        float2 rotation = Rotate2d(p.xz, angle);
        return float3(rotation.x, p.y, rotation.y);
      }

      float3 RotatePoint(float3 p, float xAxisRotation, float yAxisRotation) {
        float3 rotated = RotateAroundYAxis(p, yAxisRotation);
        return RotateAroundXAxis(rotated, xAxisRotation);
      }

      float2 GetUVsForSpherePoint(float3 fragPos, float radius, float2 pointRotation) {
        float3 projectedPosition = RotatePoint(fragPos, pointRotation.x, pointRotation.y);

        // Find our UV position.
        return clamp(float2(
          (projectedPosition.x + radius) / (2.0 * radius),
          (projectedPosition.y + radius) / (2.0 * radius)), 0, 1);
      }

      float2 Calculate2DCords(float3 spherePoint) {
        float yPercent = spherePoint.y / 2.0 + .5;
        float anglePercent = Atan2Positive(spherePoint.z, spherePoint.x) / _2_PI;
        return float2(anglePercent, yPercent);
      }

      inline float4 GetStarMetadata(float2 cords, sampler2D starData) {
        return tex2D(starData, float2(.5 + .5 * cords.x, .5 + .5 * cords.y));
      }

      inline float4 NearbyStarPoint(sampler2D nearbyStarTexture, float2 cords) {
        return tex2D(nearbyStarTexture, float2(.5 * cords.x, .5 + .5 * cords.y));
      }

      float2 AnimateStarRotation(float2 starUV, float rotationSpeed, float scale, float2 pivot) {
        return Rotate2d(starUV - pivot, rotationSpeed * _Time.y * scale) + pivot;
      }

      float GetStarRadius(float noise, float maxRadius, float twinkleAmount) {
        float noisePercent = noise;
        float minRadius = clamp((1 - twinkleAmount) * maxRadius, 0, maxRadius);
        return clamp(maxRadius * noise, minRadius, maxRadius) * _HorizonScaleFactor;
      }

      uint GetSpriteTargetIndex(uint itemCount, uint animationSpeed, float seed) {
        float spriteProgress = frac((_Time.y + (10.0f * seed)) / ((float)itemCount / (float)animationSpeed));
        return (int)(itemCount * spriteProgress);
      }

      float2 GetSpriteItemSize(float2 dimensions) {
        return float2(1.0f / dimensions.x, (1.0f / dimensions.x) * (dimensions.x / dimensions.y));
      }

      float2 GetSpriteRotationOrigin(uint targetFrameIndex, float2 dimensions, float2 itemSize) {
        uint rows = (uint)dimensions.y;
        uint columns = (uint)dimensions.x;
        return float2(((float)(targetFrameIndex % columns) * itemSize.x + (itemSize.x / 2.0f)),
          (float)((rows - 1) - (targetFrameIndex / columns)) * itemSize.y + (itemSize.y / 2.0f));
      }

      float2 GetSpriteSheetCoords(float2 uv, float2 dimensions, uint targetFrameIndex, float2 itemSize, uint numItems) {
        uint rows = (uint)dimensions.y;
        uint columns = (uint)dimensions.x;

        float2 scaledUV = float2(uv.x * itemSize.x, uv.y * itemSize.y);
        float2 offset = float2(
          targetFrameIndex % columns * itemSize.x,
          ((rows - 1) - (targetFrameIndex / columns)) * itemSize.y);

        return scaledUV + offset;
      }

      half4 StarColorWithTexture(
          float3 pos,
          float2 starCoords,
          float2 starUV,
          sampler2D starTexture,
          float4 starColorTint,
          float starDensity,
          float radius,
          float twinkleAmount,
          float twinkleSpeed,
          float rotationSpeed,
          float edgeFade,
          sampler2D nearbyStarsTexture,
          float4 gridPointWithNoise) {
        float3 gridPoint = normalize(gridPointWithNoise.xyz);
        float distanceToCenter = distance(pos, gridPoint);

        half4 outputColor = tex2D(starTexture, starUV) * starColorTint;

        // Animate alpha with twinkle wave.
        half twinkleWavePercent = smoothstep(-1, 1, cos(gridPointWithNoise.w * (100 + _Time.y) * twinkleSpeed));
        outputColor.a = clamp(twinkleWavePercent, (1 - twinkleAmount), 1);

        // If it's outside the radius, zero is multiplied to clear the color values.
        return outputColor * smoothstep(radius, radius * (1 - edgeFade), distanceToCenter);
      }

      half4 StarColorNoTexture(
          float3 pos,
          float2 starCoords,
          float4 starColorTint,
          float starDensity,
          float radius,
          float twinkleAmount,
          float twinkleSpeed,
          float edgeFade,
          sampler2D nearbyStarsTexture,
          float4 gridPointWithNoise) {
        float4 starInfo = GetStarMetadata(starCoords, nearbyStarsTexture);
        float3 gridPoint = normalize(gridPointWithNoise.xyz);

        float distanceToCenter = distance(pos, gridPoint);

        // Apply a horizon scale so stars are less visible with distance.
        radius *= _HorizonScaleFactor;

        half4 outputColor = starColorTint;

        // Animate alpha with twinkle wave.
        half twinkleWavePercent = smoothstep(-1, 1, cos(gridPointWithNoise.w * (100 + _Time.y) * twinkleSpeed));
        outputColor.a = clamp(twinkleWavePercent, (1 - twinkleAmount), 1);

        // If it's outside the radius, zero is multiplied to clear the color values.
        return outputColor * smoothstep(radius, radius * (1 - edgeFade), distanceToCenter);
      }

      half4 StarColorFromAllGrids(float3 pos) {
        float2 starCoords = Calculate2DCords(pos);
        float4 nearbyStar = float4(0, 0, 0, 0);
        float4 allStarColors = float4(0, 0, 0, 0);

#ifdef STAR_LAYER_3
        nearbyStar = NearbyStarPoint(_StarLayer3DataTex, starCoords);
        if (distance(pos, nearbyStar) <= _StarLayer3MaxRadius) {
          float radius = GetStarRadius(nearbyStar.w, _StarLayer3MaxRadius, _StarLayer3TwinkleAmount);
          float4 starInfo = GetStarMetadata(starCoords, _StarLayer3DataTex);

  #ifdef STAR_LAYER_3_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, radius, starInfo.xy);
          float2 pivot = float2(.5f, .5f);
    #if STAR_LAYER_3_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_StarLayer3SpriteItemCount, _StarLayer3SpriteAnimationSpeed, nearbyStar.w);
          float2 spriteItemSize = GetSpriteItemSize(_StarLayer3SpriteDimensions);

          texUV = GetSpriteSheetCoords(texUV, _StarLayer3SpriteDimensions, spriteFrameIndex, spriteItemSize, _StarLayer3SpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _StarLayer3SpriteDimensions, spriteItemSize);
    #endif
          texUV = AnimateStarRotation(texUV, _StarLayer3RotationSpeed * nearbyStar.w, 1, pivot);

          return StarColorWithTexture(
            pos,
            starCoords,
            texUV,
            _StarLayer3Tex,
            _StarLayer3Color,
            _StarLayer3Density,
            radius,
            _StarLayer3TwinkleAmount,
            _StarLayer3TwinkleSpeed,
            _StarLayer3RotationSpeed,
            _StarLayer3EdgeFade,
            _StarLayer3DataTex,
            nearbyStar) * _StarLayer3HDRBoost;
            
  #else
          return StarColorNoTexture(
            pos,
            starCoords,
            _StarLayer3Color,
            _StarLayer3Density,
            radius,
            _StarLayer3TwinkleAmount,
            _StarLayer3TwinkleSpeed,
            _StarLayer3EdgeFade,
            _StarLayer3DataTex,
            nearbyStar) * _StarLayer3HDRBoost;
  #endif      
        }
#endif
        
#ifdef STAR_LAYER_2
        nearbyStar = NearbyStarPoint(_StarLayer2DataTex, starCoords);
        if (distance(pos, nearbyStar) <= _StarLayer2MaxRadius) {
          float radius = GetStarRadius(nearbyStar.w, _StarLayer2MaxRadius, _StarLayer2TwinkleAmount);
          float4 starInfo = GetStarMetadata(starCoords, _StarLayer2DataTex);

#ifdef STAR_LAYER_2_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, radius, starInfo.xy);
          float2 pivot = float2(.5f, .5f);
#if STAR_LAYER_2_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_StarLayer2SpriteItemCount, _StarLayer2SpriteAnimationSpeed, nearbyStar.w);
          float2 spriteItemSize = GetSpriteItemSize(_StarLayer2SpriteDimensions);

          texUV = GetSpriteSheetCoords(texUV, _StarLayer2SpriteDimensions, spriteFrameIndex, spriteItemSize, _StarLayer2SpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _StarLayer2SpriteDimensions, spriteItemSize);   
#endif
          texUV = AnimateStarRotation(texUV, _StarLayer2RotationSpeed * nearbyStar.w, 1, pivot);

          return StarColorWithTexture(
            pos,
            starCoords,
            texUV,
            _StarLayer2Tex,
            _StarLayer2Color,
            _StarLayer2Density,
            radius,
            _StarLayer2TwinkleAmount,
            _StarLayer2TwinkleSpeed,
            _StarLayer2RotationSpeed,
            _StarLayer2EdgeFade,
            _StarLayer2DataTex,
            nearbyStar) * _StarLayer2HDRBoost;

#else
          return StarColorNoTexture(
            pos,
            starCoords,
            _StarLayer2Color,
            _StarLayer2Density,
            radius,
            _StarLayer2TwinkleAmount,
            _StarLayer2TwinkleSpeed,
            _StarLayer2EdgeFade,
            _StarLayer2DataTex,
            nearbyStar) * _StarLayer2HDRBoost;
#endif      
        }
#endif

#ifdef STAR_LAYER_1
        nearbyStar = NearbyStarPoint(_StarLayer1DataTex, starCoords);
        if (distance(pos, nearbyStar) <= _StarLayer1MaxRadius) {
          float radius = GetStarRadius(nearbyStar.w, _StarLayer1MaxRadius, _StarLayer1TwinkleAmount);
          float4 starInfo = GetStarMetadata(starCoords, _StarLayer1DataTex);

#ifdef STAR_LAYER_1_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, radius, starInfo.xy);
          float2 pivot = float2(.5f, .5f);
#if STAR_LAYER_1_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_StarLayer1SpriteItemCount, _StarLayer1SpriteAnimationSpeed, nearbyStar.w);
          float2 spriteItemSize = GetSpriteItemSize(_StarLayer1SpriteDimensions);

          texUV = GetSpriteSheetCoords(texUV, _StarLayer1SpriteDimensions, spriteFrameIndex, spriteItemSize, _StarLayer1SpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _StarLayer1SpriteDimensions, spriteItemSize);
#endif
          texUV = AnimateStarRotation(texUV, _StarLayer1RotationSpeed * nearbyStar.w, 1, pivot);

          return StarColorWithTexture(
            pos,
            starCoords,
            texUV,
            _StarLayer1Tex,
            _StarLayer1Color,
            _StarLayer1Density,
            radius,
            _StarLayer1TwinkleAmount,
            _StarLayer1TwinkleSpeed,
            _StarLayer1RotationSpeed,
            _StarLayer1EdgeFade,
            _StarLayer1DataTex,
            nearbyStar) * _StarLayer1HDRBoost;

#else
          return StarColorNoTexture(
            pos,
            starCoords,
            _StarLayer1Color,
            _StarLayer1Density,
            radius,
            _StarLayer1TwinkleAmount,
            _StarLayer1TwinkleSpeed,
            _StarLayer1EdgeFade,
            _StarLayer1DataTex,
            nearbyStar) * _StarLayer1HDRBoost;
#endif      
        }
#endif
        return allStarColors;
      }
      
      inline half4 FadeStarsColor(float verticalPosition, half4 currentStar) {
        return currentStar * smoothstep(_StarFadeBegin, clamp(_StarFadeEnd, _StarFadeBegin, 1), verticalPosition);
      }

      half4 HorizonGradient(float verticalPosition) {
        half fadePercent = smoothstep(_GradientFadeBegin, _GradientFadeEnd, verticalPosition);
        return lerp(_GradientSkyLowerColor, _GradientSkyUpperColor, fadePercent);
      }

      v2f vert(appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.verticalPosition = clamp(v.vertex.y, -1, 1);
        o.smoothVertex = v.vertex;

#ifdef CLOUDS
				float3 cloudUVs = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));

        float computedHeight = lerp(_CLOUD_HEIGHT_LIMITS.x, _CLOUD_HEIGHT_LIMITS.y, 1 - _CloudHeight);
        cloudUVs.y  *= dot(float3(0, computedHeight, 0), float3(0, .2f, 0));

        // Apply the rotation for cloud direction.
				cloudUVs.xz  = Rotate2d(cloudUVs.xz, _CloudDirection);
        cloudUVs = normalize(cloudUVs);

				float cloudSpeed = _CloudSpeed * _Time;

				o.cloudUVs.xy = (cloudUVs.xz * 0.25f)  - (0.005f) + float2(cloudSpeed / 20, cloudSpeed);
				o.cloudUVs.zw = (cloudUVs.xz * 0.35f)  - (0.0065f) + float2(cloudSpeed / 20, cloudSpeed);
#endif

#ifdef HORIZON_FOG
  #ifdef GLOBAL_FOG
        UNITY_TRANSFER_FOG(o, o.vertex);
  #endif
#endif
        return o;
      }

      half4 OrbitBodyColorWithTextureUV(float3 pos, float3 orbitBodyPosition, float2 orbitBodyRotation, 
          half4 orbitBodyTintColor, float orbitBodyRadius, float orbitBodyEdgeFade, sampler2D orbitBodyTex, float2 bodyUVs) {
        half4 color = tex2D(orbitBodyTex, bodyUVs) * orbitBodyTintColor;
        
        float fragDistance = distance(orbitBodyPosition, pos);

        float fadeEnd = orbitBodyRadius * (1 - orbitBodyEdgeFade);

        return smoothstep(orbitBodyRadius, fadeEnd, fragDistance) * color;
      }

      // Alpha premultiplied into color.
      half4 OrbitBodyColorNoTexture(float3 pos, float3 orbitBodyPosition, float2 orbitBodyRotation, 
          half4 orbitBodyColor, float orbitBodyRadius, float orbitBodyEdgeFade) {
        float2 bodyUVs = GetUVsForSpherePoint(pos, orbitBodyRadius, orbitBodyRotation);
        half4 color = orbitBodyColor;
        
        float fragDistance = distance(orbitBodyPosition, pos);
        float fadeEnd = orbitBodyRadius * (1 - orbitBodyEdgeFade);

        return smoothstep(orbitBodyRadius, fadeEnd, fragDistance) * color;
      }

#if SUN
      half4 CalculateSunColor(float3 pos) {
        half4 sunColor = half4(0, 0, 0, 0);

        #ifdef SUN_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, _SunRadius, _SunComputedRotationData.xy);
          float2 pivot = float2(.5f, .5f);

        #if SUN_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_SunSpriteItemCount, _SunSpriteAnimationSpeed, 0.0f);
          float2 spriteItemSize = GetSpriteItemSize(_SunSpriteDimensions.xy);

          texUV = GetSpriteSheetCoords(texUV, _SunSpriteDimensions, spriteFrameIndex, spriteItemSize, _SunSpriteItemCount);
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _SunSpriteDimensions, spriteItemSize);
        #endif

          #ifdef SUN_ROTATION
            texUV = AnimateStarRotation(texUV, _SunRotationSpeed, 1, pivot);
            sunColor = OrbitBodyColorWithTextureUV(pos,
              _SunComputedPositionData.xyz,
              _SunComputedRotationData.xy,
              _SunColor,
              _SunRadius,
              _SunEdgeFade,
              _SunTex,
              texUV) * _SunHDRBoost;
          #else
            sunColor = OrbitBodyColorWithTextureUV(pos,
              _SunComputedPositionData.xyz,
              _SunComputedRotationData.xy,
              _SunColor,
              _SunRadius,
              _SunEdgeFade,
              _SunTex,
              texUV) * _SunHDRBoost;
          #endif
        #else
        sunColor = OrbitBodyColorNoTexture(pos,
            _SunComputedPositionData.xyz,
            _SunComputedRotationData.xy,
            _SunColor,
            _SunRadius,
            _SunEdgeFade) * _SunHDRBoost;
        #endif

        return sunColor;
      }
#endif

#if MOON
      half4 CalculateMoonColor(float3 pos) {
        half4 moonColor = half4(0, 0, 0, 0);

        #ifdef MOON_CUSTOM_TEXTURE
          float2 texUV = GetUVsForSpherePoint(pos, _MoonRadius, _MoonComputedRotationData.xy);
          float2 pivot = float2(.5f, .5f);

          #if MOON_SPRITE_SHEET
          uint spriteFrameIndex = GetSpriteTargetIndex(_MoonSpriteItemCount, _MoonSpriteAnimationSpeed, 0.0f);
          float2 spriteItemSize = GetSpriteItemSize(_MoonSpriteDimensions.xy);

          texUV = GetSpriteSheetCoords(texUV, _MoonSpriteDimensions, spriteFrameIndex, spriteItemSize, _MoonSpriteItemCount);  
          pivot = GetSpriteRotationOrigin(spriteFrameIndex, _MoonSpriteDimensions, spriteItemSize);
          #endif

          #ifdef MOON_ROTATION
          texUV = AnimateStarRotation(texUV, _MoonRotationSpeed, 1, pivot);

          moonColor = OrbitBodyColorWithTextureUV(pos,
            _MoonComputedPositionData.xyz,
            _MoonComputedRotationData.xy,
            _MoonColor,
            _MoonRadius,
            _MoonEdgeFade,
            _MoonTex,
            texUV) * _MoonHDRBoost;
          #else
            moonColor = OrbitBodyColorWithTextureUV(pos,
              _MoonComputedPositionData.xyz,
              _MoonComputedRotationData.xy,
              _MoonColor,
              _MoonRadius,
              _MoonEdgeFade,
              _MoonTex,
              texUV) * _MoonHDRBoost;
          #endif
        #else
        moonColor = OrbitBodyColorNoTexture(pos,
            _MoonComputedPositionData.xyz,
            _MoonComputedRotationData.xy,
            _MoonColor,
            _MoonRadius,
            _MoonEdgeFade) * _MoonHDRBoost;
        #endif

        return moonColor;
      }
#endif

      half4 Calculate3WayGradientBackgroundAtPosition(float verticalPosition) {
        // 3 way gradient.
        float middleGradientPosition = _GradientFadeBegin 
          + ((_GradientFadeEnd - _GradientFadeBegin) * _GradientFadeMiddlePosition);
        
        float bottomColorPercent = smoothstep(_GradientFadeBegin, middleGradientPosition, verticalPosition);
        half4 bottomMixedColor = lerp(_GradientSkyLowerColor, _GradientSkyMiddleColor, bottomColorPercent);
        bottomMixedColor *= !step(middleGradientPosition, verticalPosition);

        float topColorPercent = smoothstep(middleGradientPosition, _GradientFadeEnd, verticalPosition);
        half4 topMixedColor = lerp(_GradientSkyMiddleColor, _GradientSkyUpperColor, topColorPercent);
        topMixedColor *= step(middleGradientPosition, verticalPosition);

        return bottomMixedColor + topMixedColor;
      }

      half4 Calculate2WayGradientBackgroundAtPosition(float verticalPosition) {
        half fadePercent = smoothstep(_GradientFadeBegin, _GradientFadeEnd, verticalPosition);
        return lerp(_GradientSkyLowerColor, _GradientSkyUpperColor, fadePercent);
      }

#ifdef CLOUDS
      half4 CalculateClouds(float4 cloudUVs, float3 vertexPos, float4 backgroundColor) {
				// Cloud noise.
				float4 tex1  = tex2D(_CloudNoiseTexture, cloudUVs.xy);
				float4 tex2  = tex2D(_CloudNoiseTexture, cloudUVs.zw);
        
        float noise1 = pow(tex1.g + tex2.g, 0.25);
				float noise2 = pow(tex2.b * tex1.r, 0.5);

        // Progress in the fadeout (0 means no fadeout, 1 means full fadeout - no clouds)
        float fadeOutPercent = smoothstep(_CloudFadePosition, 1, length(vertexPos.xz));

				// Color fix.
				_CloudColor1.rgb = pow(_CloudColor1.rgb, 2.2);
        _CloudColor2.rgb = pow(_CloudColor2.rgb, 2.2);

				//Cloud finalization.
				float3 cloud1 = lerp(float3(0, 0, 0), _CloudColor2.rgb, noise1);
				float3 cloud2 = lerp(float3(0, 0, 0), _CloudColor1.rgb, noise2) * 1.5;
				float3 cloud  = lerp(cloud1, cloud2, noise1 * noise2);

        //Cloud alpha.
        float outColorAlpha = 1.0f;
        float expandedDensity = _MAX_CLOUD_COVERAGE * (1 - _CloudDensity);
				float cloudAlpha = saturate(pow(noise1 * noise2, expandedDensity)) * pow(outColorAlpha, 0.35);
        cloudAlpha *= 1 - fadeOutPercent * _CloudFadeAmount;

        float3 outColor = lerp(backgroundColor, cloud, cloudAlpha);

        return half4(outColor, 1.0f);
      }
#endif

      // Does an over alpha blend.
      half4 AlphaBlend(half4 top, half4 bottom) {
        half3 ca = top.xyz;
        float aa = top.w;
        half3 cb = bottom.xyz;
        float ab = bottom.w;

        half3 color = (ca * aa + cb * ab * (1 - aa)) / (aa + ab * (1 - aa));
        return half4(color, 1);
      }

#ifdef HORIZON_FOG
      half4 ApplyHorizonFog(half4 skyColor, float3 vertexPos) {
        float fadePercent = smoothstep(1 - _HorizonFogLength, 1, length(vertexPos.xz));
        fadePercent *= _HorizonFogDensity;
        return lerp(skyColor, _HorizonFogColor, fadePercent);
      }
#endif

      half4 frag(v2f i) : SV_Target {
#ifdef GRADIENT_BACKGROUND
        half4 background = Calculate3WayGradientBackgroundAtPosition(i.verticalPosition);
#else
        half4 background = texCUBE(_MainTex, i.smoothVertex);
#endif

        float3 normalizedSmoothVertex = normalize(i.smoothVertex);
        int isMoonPixel = 0;
        int isSunPixel = 0;
        half4 sunColor = half4(0, 0, 0, 0);
        half4 moonColor = half4(0, 0, 0, 0);
#ifdef MOON
        // Check for moon at fragment.
        isMoonPixel = distance(i.smoothVertex, _MoonComputedPositionData.xyz) <= _MoonRadius;
        moonColor = CalculateMoonColor(normalizedSmoothVertex);
        moonColor *= isMoonPixel;
#endif

#ifdef SUN
        isSunPixel = distance(i.smoothVertex, _SunComputedPositionData.xyz) <= _SunRadius;
        sunColor = CalculateSunColor(normalizedSmoothVertex);
        sunColor *= isSunPixel;
#endif

        // Star color at current position.
        half4 starColor = StarColorFromAllGrids(normalize(i.smoothVertex));
        if (isSunPixel || isMoonPixel) {
          starColor *= 0;
        }

        // Fade stars over the horizon.
        starColor = FadeStarsColor(i.verticalPosition, starColor);

        // Merge the stars over the background color.
        half4 upperSkyColor = MergeStarIntoBackground(background, starColor);

        half4 finalColor = half4(0, 0, 0, 1);

#ifdef SUN_ALPHA_BLEND
          finalColor = AlphaBlend(sunColor, upperSkyColor);
#else
          finalColor = upperSkyColor + sunColor;
#endif

#ifdef MOON_ALPHA_BLEND
          finalColor = AlphaBlend(moonColor, finalColor);
#else
          finalColor += moonColor;
#endif

#ifdef CLOUDS
        finalColor = CalculateClouds(i.cloudUVs, i.smoothVertex, finalColor);
#endif

#ifdef HORIZON_FOG
        finalColor = ApplyHorizonFog(finalColor, i.smoothVertex);
  #ifdef GLOBAL_FOG
        UNITY_APPLY_FOG(i.fogCoord, finalColor);
  #endif
#endif

        return finalColor;
      }
      ENDCG
    }
  }
  CustomEditor "DoNotModifyShaderEditor"
}
