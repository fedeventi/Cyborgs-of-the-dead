// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "dissolve"
{
	Properties
	{
		_Float0("Float 0", Range( 0 , 1)) = 0
		_GenericClouds("GenericClouds", 2D) = "white" {}
		_Float1("Float 1", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Float0;
		uniform sampler2D _GenericClouds;
		uniform float4 _GenericClouds_ST;
		uniform float _Float1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color22 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
			float4 color23 = IsGammaSpace() ? float4(0,0.4331117,1,0) : float4(0,0.1572679,1,0);
			float2 uv_GenericClouds = i.uv_texcoord * _GenericClouds_ST.xy + _GenericClouds_ST.zw;
			float4 lerpResult21 = lerp( color22 , color23 , saturate( ( ( ( 1.0 - (2.0 + (_Float0 - 0.0) * (0.0 - 2.0) / (1.0 - 0.0)) ) + tex2D( _GenericClouds, uv_GenericClouds ) ) * _Float1 ) ));
			o.Albedo = lerpResult21.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
179;454;1109;397;2271.699;656.735;5.514752;True;False
Node;AmplifyShaderEditor.RangedFloatNode;12;110.5632,504.0623;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;False;0;0;0.2639778;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;27;502.6766,271.6888;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;2;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;555.0306,-171.0888;Inherit;True;Property;_GenericClouds;GenericClouds;1;0;Create;True;0;0;False;0;-1;09450ea7e597106468ca5c0fc7a26463;09450ea7e597106468ca5c0fc7a26463;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;24;749.9688,203.8438;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;1040.303,83.04693;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;29;1161.85,367.9711;Inherit;False;Property;_Float1;Float 1;2;0;Create;True;0;0;False;0;0;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;1363.148,90.85869;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;22;1381.253,-362.2694;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;1412.195,-131.6105;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;False;0;0,0.4331117,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;17;1762.654,31.61061;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;21;2050.606,-110.7702;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2383.637,-138.6693;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;dissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;12;0
WireConnection;24;0;27;0
WireConnection;15;0;24;0
WireConnection;15;1;16;0
WireConnection;28;0;15;0
WireConnection;28;1;29;0
WireConnection;17;0;28;0
WireConnection;21;0;22;0
WireConnection;21;1;23;0
WireConnection;21;2;17;0
WireConnection;0;0;21;0
ASEEND*/
//CHKSM=4C24659EA0826F7EA502D128FCF9413AF130F8A1