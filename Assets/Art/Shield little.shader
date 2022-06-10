// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield little"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPosition1;
		};

		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 temp_cast_0 = (3.65).xxx;
			float3 vertexPos1 = temp_cast_0;
			float4 ase_screenPos1 = ComputeScreenPos( UnityObjectToClipPos( vertexPos1 ) );
			o.screenPosition1 = ase_screenPos1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color183 = IsGammaSpace() ? float4(0,0.7244357,5.278032,1) : float4(0,0.4835784,38.85424,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV215 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode215 = ( -0.61 + 1.0 * pow( 1.0 - fresnelNdotV215, 0.5 ) );
			float4 color213 = IsGammaSpace() ? float4(0,0.1058824,0.7490196,1) : float4(0,0.0109601,0.5209957,1);
			o.Albedo = ( ( ( color183 + float4( 0,0,0,0 ) ) + fresnelNode215 ) + ( color213 + float4( 0,0,0,0 ) ) ).rgb;
			float4 ase_screenPos1 = i.screenPosition1;
			float4 ase_screenPosNorm1 = ase_screenPos1 / ase_screenPos1.w;
			ase_screenPosNorm1.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm1.z : ase_screenPosNorm1.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm1.xy ));
			float distanceDepth1 = saturate( abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm1.z ) ) / ( 1.0 ) ) );
			float3 temp_cast_1 = (( ( 0.0 * 1.0 ) + ( 1.0 - saturate( distanceDepth1 ) ) )).xxx;
			o.Emission = temp_cast_1;
			o.Alpha = 0.46;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
263;554;1109;300;1450.239;189.4831;3.52205;True;True
Node;AmplifyShaderEditor.RangedFloatNode;211;-1212.905,365.0031;Inherit;False;Constant;_Float8;Float 8;3;0;Create;True;0;0;False;0;3.65;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;1;-1012.356,279.2797;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-657.1512,-87.69182;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;183;-213.9977,-896.3135;Inherit;False;Constant;_Color2;Color 2;2;1;[HDR];Create;True;0;0;False;0;0,0.7244357,5.278032,1;0.5518868,0.7574644,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;179;36.60641,-742.9116;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;168;-757.9417,286.5232;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;213;-245.6378,-663.4993;Inherit;False;Constant;_Color0;Color 0;2;1;[HDR];Create;True;0;0;False;0;0,0.1058824,0.7490196,1;0.5518868,0.7574644,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;150;-1232.319,159.3768;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;215;-334.0765,-177.9577;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-0.61;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;169;-615.8419,310.5712;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-992.3188,-16.62317;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;220;195.0779,-215.3747;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;214;-4.492035,-462.8054;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-349.426,147.0951;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;218;555.7225,-366.3916;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;177;276.8048,399.5537;Inherit;False;Constant;_Float5;Float 5;2;0;Create;True;0;0;False;0;0.46;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;921.13,35.32136;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shield little;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;68;-1488.556,-1305.256;Inherit;False;661.478;409.3135;Nodos Principales;0;;1,1,1,1;0;0
WireConnection;1;1;211;0
WireConnection;179;0;183;0
WireConnection;168;0;1;0
WireConnection;215;3;216;0
WireConnection;169;0;168;0
WireConnection;155;1;150;0
WireConnection;220;0;179;0
WireConnection;220;1;215;0
WireConnection;214;0;213;0
WireConnection;210;0;155;0
WireConnection;210;1;169;0
WireConnection;218;0;220;0
WireConnection;218;1;214;0
WireConnection;0;0;218;0
WireConnection;0;2;210;0
WireConnection;0;9;177;0
ASEEND*/
//CHKSM=92F31208CB875589108019841C958A4C9335732F