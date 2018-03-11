Shader "VolumetricCloud"
{
	Properties
	{
	    [NoScaleOffset]_PerlinNormalMap ("Perlin Normal Map", 2D) = "white" {}

		[Header(Colors)]
			_BaseColor ("Base Color", Color) = (1,1,1,1)
			_Shading ("Shading Color", Color) = (0, 0, 0, 1)
			[Toggle(_NORMALMAP)] _NormalmapState ("Use Normalmap", int) = 1
				[ToggleHideDrawer(_NormalmapState)] _NormalsIntensity ("Normals Intensity", float ) = 1
				[ToggleHideDrawer(_NormalmapState)] _IndirectContribution("Indirect Lighting", float) = 1
				[ToggleHideDrawer(_NormalmapState)] _Normalized ("Normalized", float ) = 0
			_DepthColor ("Depth Intensity", float ) = 0
			_LightAttenuation ("Light Attenuation", Color) = (.35,.35,.35,.15)
			_DistanceBlend ("Distance Blend", float ) = 0.6
			[Toggle(_RENDERSHADOWS)] _SSShadows ("Screen Space Shadows", int) = 0
				[ToggleHideDrawer(_SSShadows)] _ShadowColor ("Shadow Color", Color) = (0,0,0,.5)
				[ToggleHideDrawer(_SSShadows)] _ShadowDrawDistance ("Draw Distance", float) = 999
				[Toggle(_RENDERSHADOWSONLY)] _RenderShadowsOnly ("Render Shadows Only", int) = 0
			//#if UNITY_VERSION >= 540
				[Toggle(_HQPOINTLIGHT)] _HQPointLight ("High quality point light", int) = 0
			//#endif


        [Header(Shape)]
			_Density ("Density", float ) = 0
			[Toggle(_DENSITYMAP)] _DensityMapOn ("Density Map", int) = 0
				[ToggleHideDrawer(_DensityMapOn)] _DensityMap ("Density Map", 2D) = "white" {}
				[ToggleHideDrawer(_DensityMapOn)] _DensityLow ("Density Low", float) = -1
				[ToggleHideDrawer(_DensityMapOn)] _DensityHigh ("Density High", float) = 1
			_Alpha ("Alpha", float ) = 4
			_AlphaCut ("AlphaCut", float ) = 0.01
        
		[Header(Animation)]
			_TimeMult ("Speed", float ) = 0.1
			_TimeMultSecondLayer ("Speed Second Layer", float ) = 4
			_WindDirection ("Wind Direction", vector) = (1,0,0,0)

		[Header(Dimensions)]
			_CloudTransform("Cloud Transform", vector) =  (100, 20, 0, 0)
			[Toggle(_SPHEREMAPPED)] _SphereMapped ("Spherical", int) = 0
				[ToggleHideDrawer(_SphereMapped)] _CloudSpherePosition("Sphere position", vector) =  (0, 0, 0, 0)
				[ToggleHideDrawer(_SphereMapped)] _SphereHorizonStretch("Sphere stretch horizon", float) = .6
				//[ToggleHideDrawer(_SphereMapped)] _CloudSphereDimensions("Cloud Sphere dimensions", vector) =  (1, 1, 1, 0)
			_Tiling ("Tiling", float ) = 1500
			_OrthographicPerspective ("Orthographic Perspective", float) = 0

		[Space(10)]

		[Header(Raymarcher)]
			_DrawDistance("Draw distance", float) = 1000.0
			_MaxSteps("Steps Max", int) = 500
			_StepSize("Step Size", float) = 0.015
			_StepSkip("Step Skip", float) = 10
			_StepNearSurface("Step near surface", float) = 0.5
			_LodBase("Lod Base", float) = 0
			_LodOffset("Lod Offset", float) = 0.7
			_OpacityGain("Opacity Gain", float) = 0.1
			[KeywordEnumFull(_, _SKIPPIXELONCE, _SKIPPIXELTWICE)] _SkipPixel ("Skip Pixel", int) = 0

		[Space(10)]

		[Header(Debug)]
			_RenderQueue("Render Queue", Range(0, 5000)) = 2501
			//[Toggle]_ZWrite ("ZWrite", int) = 0
			//[HideInInspector] _Cull ("Culling", Range(0, 2)) = 2.0
			//_Mode ("__mode", float) = 0.0
			//[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend", float) = 5.0
			//[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dist Blend", float) = 10.0
		
		[HideInInspector][KeywordEnumFull(_, _ALPHATEST_ON, _ALPHABLEND_ON, _ALPHAPREMULTIPLY_ON)] _AlphaMode ("Alpha Mode", int) = 2

		
	}

	CGINCLUDE
		#define UNITY_SETUP_BRDF_INPUT MetallicSetup
	ENDCG

	SubShader
	{
		Tags { "RenderType"="Transparent" "PreviewType"="Plane"/*"PerformanceChecks"="False"*/}
		LOD 300
	

		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
			Name "FORWARD" 
			Tags { "LightMode"="ForwardBase" "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
			Blend SrcAlpha OneMinusSrcAlpha//[_SrcBlend] [_DstBlend]
			ZWrite Off//[_ZWrite]
			Cull Back//[_Cull]
			
			CGPROGRAM
			#pragma target 3.0
			
			// -------------------------------------
					
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _RENDERSHADOWS
			#pragma shader_feature _RENDERSHADOWSONLY
			#pragma shader_feature _HQPOINTLIGHT
			#pragma shader_feature _ _SKIPPIXELONCE _SKIPPIXELTWICE
			#pragma shader_feature _DENSITYMAP
			#pragma shader_feature _SPHEREMAPPED

			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICGLOSSMAP 
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex vertForwardModified
			#pragma fragment fragForwardClouds

			#include "VolumetricCloudsCG.cginc"

			ENDCG
		}
		/*
		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
			Name "FORWARD_DELTA"
			Tags { "LightMode" = "ForwardAdd" }
			Blend [_SrcBlend] One
			Fog { Color (0,0,0,0) } // in additive pass fog should be black
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			// GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles

			// -------------------------------------

			
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP
			
			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog

			#pragma vertex vertAdd
			#pragma fragment fragAdd
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}
		
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers gles
			
			// -------------------------------------


			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
		
		// ------------------------------------------------------------------
		//  Deferred pass
		Pass
		{
			Name "DEFERRED"
			Tags { "LightMode" = "Deferred" }

			CGPROGRAM
			#pragma target 3.0
			// TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
			#pragma exclude_renderers nomrt gles
			

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#pragma multi_compile ___ UNITY_HDR_ON
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
			#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
			
			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "UnityStandardCore.cginc"

			ENDCG
		}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		
		Pass
		{
			Name "META" 
			Tags { "LightMode"="Meta" }

			Cull Off

			CGPROGRAM
			#pragma vertex vert_meta
			#pragma fragment frag_meta

			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2

			#include "UnityStandardMeta.cginc"
			ENDCG
		}*/
	}
	/*
	SubShader
	{
		Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
		LOD 150

		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
			Name "FORWARD" 
			Tags { "LightMode" = "ForwardBase" }

			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]

			CGPROGRAM
			#pragma target 2.0
			
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION 
			#pragma shader_feature _METALLICGLOSSMAP 
			#pragma shader_feature ___ _DETAIL_MULX2
			// SM2.0: NOT SUPPORTED shader_feature _PARALLAXMAP

			#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma vertex vertBase
			#pragma fragment fragBase
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
			Name "FORWARD_DELTA"
			Tags { "LightMode" = "ForwardAdd" }
			Blend [_SrcBlend] One
			Fog { Color (0,0,0,0) } // in additive pass fog should be black
			ZWrite Off
			ZTest LEqual
			
			CGPROGRAM
			#pragma target 2.0

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			// SM2.0: NOT SUPPORTED shader_feature _PARALLAXMAP
			#pragma skip_variants SHADOWS_SOFT
			
			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog
			
			#pragma vertex vertAdd
			#pragma fragment fragAdd
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 2.0

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma skip_variants SHADOWS_SOFT
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
			Name "META" 
			Tags { "LightMode"="Meta" }

			Cull Off

			CGPROGRAM
			#pragma vertex vert_meta
			#pragma fragment frag_meta

			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2

			#include "UnityStandardMeta.cginc"
			ENDCG
		}
	}*/


	//FallBack "VertexLit"
	CustomEditor "VCloudShaderGUI"
}
