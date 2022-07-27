// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shield little"
{
	Properties
	{
		_Float5("Float 5", Range( 0 , 1)) = 0.46
		[HDR]_Color0("Color 0", Color) = (0,2.996078,0.2818567,1)
		[HDR]_Color2("Color 2", Color) = (0,4.954613,5.278031,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPosition1;
		};

		uniform float4 _Color2;
		uniform float4 _Color0;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Float5;


		float2 voronoihash233( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi233( float2 v, float time, inout float2 id, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash233( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F2 - F1;
		}


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
			float time233 = 0.0;
			float4 appendResult236 = (float4((0.5 + (sin( _Time.y ) - -1.0) * (0.56 - 0.5) / (1.0 - -1.0)) , 1.0 , 0.0 , 0.0));
			float2 uv_TexCoord232 = i.uv_texcoord * appendResult236.xy;
			float2 panner234 = ( 0.2 * _Time.y * float2( 0,1 ) + uv_TexCoord232);
			float2 coords233 = panner234 * 15.2;
			float2 id233 = 0;
			float voroi233 = voronoi233( coords233, time233,id233, 0 );
			float4 lerpResult227 = lerp( _Color2 , _Color0 , voroi233);
			o.Albedo = lerpResult227.rgb;
			float4 ase_screenPos1 = i.screenPosition1;
			float4 ase_screenPosNorm1 = ase_screenPos1 / ase_screenPos1.w;
			ase_screenPosNorm1.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm1.z : ase_screenPosNorm1.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm1.xy ));
			float distanceDepth1 = saturate( abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm1.z ) ) / ( 1.0 ) ) );
			float3 temp_cast_2 = (( ( 0.0 * 1.0 ) + ( 1.0 - saturate( distanceDepth1 ) ) )).xxx;
			o.Emission = temp_cast_2;
			o.Alpha = _Float5;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
179;454;1109;397;2122.911;1237.394;2.533185;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;239;-2044.239,-339.8378;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;238;-1831.436,-327.1846;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;240;-1691.101,-313.3811;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;0.56;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-1212.905,365.0031;Inherit;False;Constant;_Float8;Float 8;3;0;Create;True;0;0;False;0;3.65;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;236;-1398.927,-275.4215;Inherit;False;FLOAT4;4;0;FLOAT;0.5;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DepthFade;1;-1012.356,279.2797;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;232;-1210.96,-291.4135;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.69,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;168;-757.9417,286.5232;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-1232.319,159.3768;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;234;-915.8061,-292.6759;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VoronoiNode;233;-622.2029,-292.55;Inherit;True;0;0;1;2;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;15.2;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.ColorNode;213;-523.4847,-623.5872;Inherit;False;Property;_Color0;Color 0;1;1;[HDR];Create;True;0;0;False;0;0,2.996078,0.2818567,1;0,0.04616845,2.739736,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;169;-615.8419,310.5712;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-992.3188,-16.62317;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;183;-211.1129,-896.3135;Inherit;False;Property;_Color2;Color 2;2;1;[HDR];Create;True;0;0;False;0;0,4.954613,5.278031,1;0,1.854412,5.278031,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-349.426,147.0951;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;227;189.6878,-181.7732;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;177;276.8048,399.5537;Inherit;False;Property;_Float5;Float 5;0;0;Create;True;0;0;False;0;0.46;0.7847059;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;921.13,35.32136;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shield little;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;68;-1488.556,-1305.256;Inherit;False;661.478;409.3135;Nodos Principales;0;;1,1,1,1;0;0
WireConnection;238;0;239;0
WireConnection;240;0;238;0
WireConnection;236;0;240;0
WireConnection;1;1;211;0
WireConnection;232;0;236;0
WireConnection;168;0;1;0
WireConnection;234;0;232;0
WireConnection;233;0;234;0
WireConnection;169;0;168;0
WireConnection;155;1;150;0
WireConnection;210;0;155;0
WireConnection;210;1;169;0
WireConnection;227;0;183;0
WireConnection;227;1;213;0
WireConnection;227;2;233;0
WireConnection;0;0;227;0
WireConnection;0;2;210;0
WireConnection;0;9;177;0
ASEEND*/
//CHKSM=27BEECF051BCE74464FF5B27C141D16EB1E5B0D4