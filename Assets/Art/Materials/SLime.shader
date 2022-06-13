// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SLime"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 12.3
		_Float1("Float 1", Range( 0 , 1)) = 0
		_Float2("Float 2", Range( 0 , 1)) = 0
		_Float3("Float 3", Range( 0 , 30)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _Float1;
		uniform float _Float2;
		uniform float _Float3;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime20 = _Time.y * _Float2;
			float3 lerpResult17 = lerp( float3( 0,0,0 ) , ( ase_vertex3Pos * _Float1 ) , saturate( ( sin( ( ( ase_vertex3Pos.y + mulTime20 ) * _Float3 ) ) + sin( ( ( ase_vertex3Pos.x + mulTime20 ) * _Float3 ) ) + sin( ( ( ase_vertex3Pos.z + mulTime20 ) * _Float3 ) ) ) ));
			v.vertex.xyz += lerpResult17;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color4 = IsGammaSpace() ? float4(0.5711935,1.233551,0.02909317,0) : float4(0.285953,1.586885,0.002251794,0);
			o.Emission = color4.rgb;
			o.Smoothness = 0.94;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV5 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode5 = ( 0.0 + 0.27 * pow( 1.0 - fresnelNdotV5, 0.87 ) );
			o.Alpha = saturate( fresnelNode5 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
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
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
366;442;1109;535;2024.464;739.9102;2.311522;True;False
Node;AmplifyShaderEditor.RangedFloatNode;22;-1019.284,662.4614;Inherit;False;Property;_Float2;Float 2;6;0;Create;True;0;0;False;0;0;0.4144679;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;20;-747.3873,663.1573;Inherit;False;1;0;FLOAT;4.63;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;16;-757.6728,469.829;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-492.5855,719.8859;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-541.934,1026.629;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-495.7906,541.9611;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-791.2449,858.9465;Inherit;False;Property;_Float3;Float 3;7;0;Create;True;0;0;False;0;0;18.01497;0;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-359.5424,764.7678;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;31.68;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-390.2603,992.9672;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;31.68;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-344.117,508.2995;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;31.68;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;31;-207.5262,1005.791;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;23;-178.4113,767.9738;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;11;-161.3828,521.123;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-490.9821,392.8885;Inherit;False;Property;_Float1;Float 1;5;0;Create;True;0;0;False;0;0;0.05565901;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;94.08662,577.2253;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;323.6654,570.8378;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;5;-934.8417,165.4624;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0.27;False;3;FLOAT;0.87;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-120.7054,373.6535;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-537.3159,-7.653941;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;0.94;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;6;-468.3893,141.4185;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;17;447.8474,370.4593;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;4;-625.476,-336.2548;Inherit;False;Constant;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;0.5711935,1.233551,0.02909317,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;867.1288,-16.0173;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;SLime;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;12.3;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;22;0
WireConnection;26;0;16;1
WireConnection;26;1;20;0
WireConnection;29;0;16;3
WireConnection;29;1;20;0
WireConnection;21;0;16;2
WireConnection;21;1;20;0
WireConnection;25;0;26;0
WireConnection;25;1;28;0
WireConnection;30;0;29;0
WireConnection;30;1;28;0
WireConnection;15;0;21;0
WireConnection;15;1;28;0
WireConnection;31;0;30;0
WireConnection;23;0;25;0
WireConnection;11;0;15;0
WireConnection;24;0;11;0
WireConnection;24;1;23;0
WireConnection;24;2;31;0
WireConnection;27;0;24;0
WireConnection;18;0;16;0
WireConnection;18;1;19;0
WireConnection;6;0;5;0
WireConnection;17;1;18;0
WireConnection;17;2;27;0
WireConnection;0;2;4;0
WireConnection;0;4;7;0
WireConnection;0;9;6;0
WireConnection;0;11;17;0
ASEEND*/
//CHKSM=97B3ECBC9352A95E212F1168AC9B1817AB77D7CB