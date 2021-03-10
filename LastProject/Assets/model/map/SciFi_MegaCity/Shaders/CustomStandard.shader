// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Standard"
{
	Properties
	{
		[Header(Main)][Space(10)]_Color("Color", Color) = (1,1,1,0)
		_MainTex("Albedo", 2D) = "white" {}
		[Space(10)][Header(Normal)][Space(10)]_BumpMap("Normal", 2D) = "bump" {}
		_BumpScale("Normal Power", Range( 0 , 1)) = 1
		[Space(10)][Header(Metallic  Smoothness)][Space(10)]_MetallicGlossMap("Metallic (R) Smoothness (A)", 2D) = "white" {}
		_Glossiness("Smoothness Power", Range( 0 , 1)) = 1
		_Metallic("Metallic Power", Range( 0 , 1)) = 1
		[Space(10)][Header(Occlusion)][Space(10)]_OcclusionMap("Occlusion", 2D) = "white" {}
		_OcclusionStrength("Occlusion Power", Range( 0 , 1)) = 0.5
		[HDR][Space(10)][Header(Emission)][Space(10)]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
		[Toggle]_ConvertMaptoGrayscale("Convert Map to Grayscale", Float) = 0
		[Space(10)][Header(Parallax)][Space(10)]_Heightmap("Heightmap", 2D) = "white" {}
		_BumpScale2("Parallax Power", Range( 0 , 1)) = 0.015
		_BumpScale3("Parallax Bias", Range( 0 , 1)) = 0.5
		[Space(10)][Header(Masked Color)][Space(10)]_ColorMask01("Color 01", Color) = (1,1,1,0)
		_ColorMask02("Color 02", Color) = (1,1,1,0)
		_Mask("Mask", 2D) = "white" {}
		_BumpScale1("Mask Faloff", Range( 0.001 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			float3 worldNormal;
			float3 worldPos;
		};

		uniform float _BumpScale;
		uniform sampler2D _BumpMap;
		uniform sampler2D _Heightmap;
		uniform float _BumpScale2;
		uniform float _BumpScale3;
		uniform float4 _Heightmap_ST;
		uniform float4 _Color;
		uniform float4 _ColorMask01;
		uniform float4 _ColorMask02;
		uniform sampler2D _Mask;
		uniform float _BumpScale1;
		uniform sampler2D _MainTex;
		uniform float4 _EmissionColor;
		uniform float _ConvertMaptoGrayscale;
		uniform sampler2D _EmissionMap;
		uniform sampler2D _MetallicGlossMap;
		uniform float _Metallic;
		uniform float _Glossiness;
		uniform sampler2D _OcclusionMap;
		uniform float _OcclusionStrength;


		inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv, int index )
		{
			float3 result = 0;
			int stepIndex = 0;
			int numSteps = ( int )lerp( (float)maxSamples, (float)minSamples, saturate( dot( normalWorld, viewWorld ) ) );
			float layerHeight = 1.0 / numSteps;
			float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
			uvs += refPlane * plane;
			float2 deltaTex = -plane * layerHeight;
			float2 prevTexOffset = 0;
			float prevRayZ = 1.0f;
			float prevHeight = 0.0f;
			float2 currTexOffset = deltaTex;
			float currRayZ = 1.0f - layerHeight;
			float currHeight = 0.0f;
			float intersection = 0;
			float2 finalTexOffset = 0;
			while ( stepIndex < numSteps + 1 )
			{
				currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
				if ( currHeight > currRayZ )
				{
					stepIndex = numSteps + 1;
				}
				else
				{
					stepIndex++;
					prevTexOffset = currTexOffset;
					prevRayZ = currRayZ;
					prevHeight = currHeight;
					currTexOffset += deltaTex;
					currRayZ -= layerHeight;
				}
			}
			int sectionSteps = 10;
			int sectionIndex = 0;
			float newZ = 0;
			float newHeight = 0;
			while ( sectionIndex < sectionSteps )
			{
				intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
				finalTexOffset = prevTexOffset + intersection * deltaTex;
				newZ = prevRayZ - intersection * layerHeight;
				newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
				if ( newHeight > newZ )
				{
					currTexOffset = finalTexOffset;
					currHeight = newHeight;
					currRayZ = newZ;
					deltaTex = intersection * deltaTex;
					layerHeight = intersection * layerHeight;
				}
				else
				{
					prevTexOffset = finalTexOffset;
					prevHeight = newHeight;
					prevRayZ = newZ;
					deltaTex = ( 1 - intersection ) * deltaTex;
					layerHeight = ( 1 - intersection ) * layerHeight;
				}
				sectionIndex++;
			}
			return uvs + finalTexOffset;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float2 OffsetPOM26 = POM( _Heightmap, i.uv_texcoord, ddx(i.uv_texcoord), ddy(i.uv_texcoord), ase_worldNormal, ase_worldViewDir, i.viewDir, 128, 128, _BumpScale2, _BumpScale3, _Heightmap_ST.xy, float2(0,0), 0 );
			o.Normal = UnpackScaleNormal( tex2D( _BumpMap, OffsetPOM26 ), _BumpScale );
			float4 lerpResult21 = lerp( _ColorMask01 , _ColorMask02 , ( 1.0 - pow( tex2D( _Mask, OffsetPOM26 ).r , _BumpScale1 ) ));
			o.Albedo = ( ( _Color * lerpResult21 ) * tex2D( _MainTex, OffsetPOM26 ) ).rgb;
			float4 tex2DNode32 = tex2D( _EmissionMap, OffsetPOM26 );
			float grayscale36 = Luminance(tex2DNode32.rgb);
			float4 temp_cast_2 = (grayscale36).xxxx;
			o.Emission = ( _EmissionColor * (( _ConvertMaptoGrayscale )?( temp_cast_2 ):( tex2DNode32 )) ).rgb;
			float4 tex2DNode8 = tex2D( _MetallicGlossMap, OffsetPOM26 );
			o.Metallic = ( tex2DNode8.r * _Metallic );
			o.Smoothness = ( tex2DNode8.a * _Glossiness );
			o.Occlusion = pow( tex2D( _OcclusionMap, OffsetPOM26 ).r , _OcclusionStrength );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
-2560;0;2560;1379;2631.544;35.68494;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;31;-1658.692,791.9723;Inherit;False;Property;_BumpScale3;Parallax Bias;14;0;Create;False;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1600,384;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-1658.692,707.9723;Inherit;False;Property;_BumpScale2;Parallax Power;13;0;Create;False;0;0;False;0;0.015;0.015;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;30;-1575.692,925.9723;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;28;-1609,507;Inherit;True;Property;_Heightmap;Heightmap;12;0;Create;True;0;0;False;3;Space(10);Header(Parallax);Space(10);None;863bdbd65c9aaa64eb369aec3e3a7231;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;26;-1303.966,724.4091;Inherit;False;0;128;False;-1;128;False;-1;10;0.02;0;False;1,1;False;0,0;Texture2D;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;11;-912.8998,-123.5001;Inherit;True;Property;_Mask;Mask;17;0;Create;True;0;0;False;1;;-1;None;8e8f9eb23017aab439c862236b6d106b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-904.6917,88.80737;Inherit;False;Property;_BumpScale1;Mask Faloff;18;0;Create;False;0;0;False;0;1;1;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;24;-483.6917,55.80737;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-825,-974;Inherit;False;Property;_ColorMask01;Color 01;15;0;Create;False;0;0;False;3;Space(10);Header(Masked Color);Space(10);1,1,1,0;0.509434,0.509434,0.509434,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-824.6021,-793.603;Inherit;False;Property;_ColorMask02;Color 02;16;0;Create;False;0;0;False;0;1,1,1,0;0.2924528,0.2924528,0.2924528,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;32;-880.792,1340.66;Inherit;True;Property;_EmissionMap;Emission;10;0;Create;False;0;0;False;0;-1;None;78bbf745bf614b547b7ea7c0b26cf9fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;22;-548.3667,-100.7951;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;36;-562.792,1456.66;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-373,-909;Inherit;False;Property;_Color;Color;0;0;Create;False;0;0;False;2;Header(Main);Space(10);1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;21;-400,-688;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-544,1040;Inherit;False;Property;_OcclusionStrength;Occlusion Power;8;0;Create;False;0;0;False;0;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;37;-366.792,1341.66;Inherit;False;Property;_ConvertMaptoGrayscale;Convert Map to Grayscale;11;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-544,704;Inherit;False;Property;_Glossiness;Smoothness Power;5;0;Create;False;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-544,576;Inherit;False;Property;_Metallic;Metallic Power;6;0;Create;False;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1209.216,318.5922;Inherit;False;Property;_BumpScale;Normal Power;3;0;Create;False;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-912,896;Inherit;True;Property;_OcclusionMap;Occlusion;7;0;Create;False;0;0;False;3;Space(10);Header(Occlusion);Space(10);-1;None;e85e047268deccf4b82befaace24a3a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-912,-368;Inherit;True;Property;_MainTex;Albedo;1;0;Create;False;0;0;False;0;-1;None;d7cc9097e14e62b449be27d773771cb4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-82.60205,-658.603;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;8;-912,512;Inherit;True;Property;_MetallicGlossMap;Metallic (R) Smoothness (A);4;0;Create;False;0;0;False;3;Space(10);Header(Metallic  Smoothness);Space(10);-1;None;3087b0998cfd66d4283cbcc8549610de;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;33;-880.792,1159.66;Inherit;False;Property;_EmissionColor;Emission Color;9;1;[HDR];Create;False;0;0;False;3;Space(10);Header(Emission);Space(10);0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-912,256;Inherit;True;Property;_BumpMap;Normal;2;0;Create;False;0;0;False;3;Space(10);Header(Normal);Space(10);-1;None;b5f2735734a043e48bc40d5ed05db1a1;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;23;-256,944;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-240,640;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-240,512;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-112,-386.5996;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-101.792,1182.66;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;48,256;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Standard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;27;0
WireConnection;26;1;28;0
WireConnection;26;2;29;0
WireConnection;26;3;30;0
WireConnection;26;4;31;0
WireConnection;11;1;26;0
WireConnection;24;0;11;1
WireConnection;24;1;25;0
WireConnection;32;1;26;0
WireConnection;22;0;24;0
WireConnection;36;0;32;0
WireConnection;21;0;12;0
WireConnection;21;1;38;0
WireConnection;21;2;22;0
WireConnection;37;0;32;0
WireConnection;37;1;36;0
WireConnection;6;1;26;0
WireConnection;5;1;26;0
WireConnection;39;0;9;0
WireConnection;39;1;21;0
WireConnection;8;1;26;0
WireConnection;7;1;26;0
WireConnection;7;5;14;0
WireConnection;23;0;6;1
WireConnection;23;1;20;0
WireConnection;15;0;8;4
WireConnection;15;1;17;0
WireConnection;16;0;8;1
WireConnection;16;1;18;0
WireConnection;10;0;39;0
WireConnection;10;1;5;0
WireConnection;34;0;33;0
WireConnection;34;1;37;0
WireConnection;0;0;10;0
WireConnection;0;1;7;0
WireConnection;0;2;34;0
WireConnection;0;3;16;0
WireConnection;0;4;15;0
WireConnection;0;5;23;0
ASEEND*/
//CHKSM=8E0662C24D039ED79DD715A729F8D91466B112C9