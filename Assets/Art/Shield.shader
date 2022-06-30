// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Parcial 2/Shield"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Float0("Float 0", Range( 0 , 4)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _Float0;
		uniform sampler2D _MainTexture;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color209 = IsGammaSpace() ? float4(0,0.6155989,1.414214,1) : float4(0,0.3370582,2.143547,1);
			float4 color183 = IsGammaSpace() ? float4(0.8392157,1.45098,1.498039,1) : float4(0.6724432,2.268065,2.43305,1);
			float temp_output_216_0 = ( ( ( 1.0 - i.uv_texcoord.y ) + -0.76 ) * _Float0 );
			float4 lerpResult217 = lerp( color209 , color183 , saturate( temp_output_216_0 ));
			o.Emission = lerpResult217.rgb;
			float2 temp_cast_1 = (1.0).xx;
			float2 uv_TexCoord153 = i.uv_texcoord * temp_cast_1;
			float2 panner154 = ( 1.0 * _Time.y * float2( 0,0.08 ) + uv_TexCoord153);
			float4 color162 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth1 = saturate( abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 3.0 ) ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV21 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode21 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV21, 0.5 ) );
			o.Alpha = saturate( ( temp_output_216_0 + ( ( ( ( ( tex2D( _MainTexture, panner154 ).g * 1.0 ) * color162 ) + float4( 0,0,0,0 ) ) + ( 1.0 - saturate( distanceDepth1 ) ) ) + fresnelNode21 ) ) ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
412;467;1109;397;2221.659;818.2316;3.296946;True;False
Node;AmplifyShaderEditor.RangedFloatNode;152;-3312,-576;Inherit;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;153;-3104,-608;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;156;-3020,-424;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;0,0.08;0.5,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;154;-2752,-576;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-2336,-320;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;151;-2512,-560;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;-1;None;a7cb9a51de439b942b87574ed9d75a4e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;211;-1212.905,365.0031;Inherit;False;Constant;_Float8;Float 8;3;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-2096,-496;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;162;-2122.409,-846.0754;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,1,1,1;0.0238519,0.03603184,0.0754717,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;1;-1012.356,279.2797;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;213;-1104.779,-379.9615;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-1476.08,-264.0023;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;168;-757.9417,286.5232;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;177;-399.0747,470.6993;Inherit;False;Constant;_Float5;Float 5;2;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;169;-615.8419,310.5712;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;215;-861.5063,-371.715;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;166;-793.438,30.89244;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;21;-117.4601,388.1896;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-417.4092,161.6629;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-522.9552,-6.682148;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;3.561549;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;214;-669.7751,-212.9698;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.76;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-253.0685,-74.77339;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;3.45;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;212;188.0658,247.6014;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;218;66.16322,-41.14967;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;219;512.4995,217.2489;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;183;-526.4437,-393.11;Inherit;False;Constant;_Color2;Color 2;2;1;[HDR];Create;True;0;0;False;0;0.8392157,1.45098,1.498039,1;0.5518868,0.7574644,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;209;-107.0987,-335.7279;Inherit;False;Constant;_Color4;Color 4;3;1;[HDR];Create;True;0;0;False;0;0,0.6155989,1.414214,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;217;338.2892,-123.6262;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;220;725.2725,227.3261;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1050.351,-9.795948;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Parcial 2/Shield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;68;-1488.556,-1305.256;Inherit;False;661.478;409.3135;Nodos Principales;0;;1,1,1,1;0;0
WireConnection;153;0;152;0
WireConnection;154;0;153;0
WireConnection;154;2;156;0
WireConnection;151;1;154;0
WireConnection;155;0;151;2
WireConnection;155;1;150;0
WireConnection;1;0;211;0
WireConnection;163;0;155;0
WireConnection;163;1;162;0
WireConnection;168;0;1;0
WireConnection;169;0;168;0
WireConnection;215;0;213;2
WireConnection;166;0;163;0
WireConnection;21;3;177;0
WireConnection;210;0;166;0
WireConnection;210;1;169;0
WireConnection;214;0;215;0
WireConnection;216;0;214;0
WireConnection;216;1;221;0
WireConnection;212;0;210;0
WireConnection;212;1;21;0
WireConnection;218;0;216;0
WireConnection;219;0;216;0
WireConnection;219;1;212;0
WireConnection;217;0;209;0
WireConnection;217;1;183;0
WireConnection;217;2;218;0
WireConnection;220;0;219;0
WireConnection;0;2;217;0
WireConnection;0;9;220;0
ASEEND*/
//CHKSM=3D1D6775FA1F0B771AC88CDD73F6C6C9B824D65F