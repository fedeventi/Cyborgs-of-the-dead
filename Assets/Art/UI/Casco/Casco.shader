// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Casco"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_ToxicityValue("ToxicityValue", Range( 0 , 1)) = 0
		_hitValue("hitValue", Range( 0 , 1)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Casco3LowHealtMode("Casco3LowHealtMode", 2D) = "white" {}
		[Toggle]_LowLife("LowLife", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform sampler2D _Casco3LowHealtMode;
			uniform float4 _Casco3LowHealtMode_ST;
			uniform sampler2D _TextureSample2;
			uniform float4 _TextureSample2_ST;
			uniform float _LowLife;
			uniform sampler2D _TextureSample1;
			uniform float4 _TextureSample1_ST;
			uniform float _ToxicityValue;
			uniform float _hitValue;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv_TextureSample0 = IN.texcoord.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float4 tex2DNode2 = tex2D( _TextureSample0, uv_TextureSample0 );
				float2 uv_Casco3LowHealtMode = IN.texcoord.xy * _Casco3LowHealtMode_ST.xy + _Casco3LowHealtMode_ST.zw;
				float4 tex2DNode28 = tex2D( _Casco3LowHealtMode, uv_Casco3LowHealtMode );
				float mulTime32 = _Time.y * 3.0;
				float temp_output_35_0 = (0.0 + (sin( mulTime32 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
				float4 lerpResult39 = lerp( tex2DNode2 , tex2DNode28 , temp_output_35_0);
				float2 uv_TextureSample2 = IN.texcoord.xy * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
				float4 tex2DNode26 = tex2D( _TextureSample2, uv_TextureSample2 );
				float4 lerpResult29 = lerp( tex2DNode28 , tex2DNode26 , tex2DNode26.a);
				float4 lerpResult30 = lerp( lerpResult39 , lerpResult29 , temp_output_35_0);
				float4 lerpResult25 = lerp( tex2DNode2 , lerpResult30 , (( _LowLife )?( 1.0 ):( 0.0 )));
				float4 lerpResult38 = lerp( tex2DNode2 , tex2DNode28 , (( _LowLife )?( 1.0 ):( 0.0 )));
				float2 uv_TextureSample1 = IN.texcoord.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
				float mulTime20 = _Time.y * 5.0;
				float4 lerpResult24 = lerp( lerpResult38 , tex2D( _TextureSample1, uv_TextureSample1 ) , (0.0 + (sin( mulTime20 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)));
				float4 lerpResult5 = lerp( lerpResult25 , lerpResult24 , _ToxicityValue);
				float4 color15 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
				float4 appendResult16 = (float4(color15.r , color15.g , color15.b , tex2DNode2.a));
				float4 lerpResult13 = lerp( lerpResult5 , appendResult16 , _hitValue);
				
				half4 color = lerpResult13;
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
338;471;1109;535;2706.408;-239.493;1.976844;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;32;-2068.245,1217.162;Inherit;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;33;-1875.173,1181.506;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1508.757,-34.91629;Inherit;False;1;0;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;35;-1636.44,1008.234;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;28;-2592.535,568.3244;Inherit;True;Property;_Casco3LowHealtMode;Casco3LowHealtMode;5;0;Create;True;0;0;False;0;-1;2b1f26e57fdfa9c41b8c860b9d7409f9;2b1f26e57fdfa9c41b8c860b9d7409f9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-2514.833,1013.204;Inherit;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;False;0;-1;None;905bac1a2b7734c46a14d142f2d241f8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-2072.433,306.1328;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;06aacd925f93ca44c8dc07bfe1155268;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;21;-1300.994,-36.56056;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;40;-1347.362,824.1871;Inherit;False;Property;_LowLife;LowLife;7;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;29;-1864.425,816.4664;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;39;-1785.151,626.1418;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1097.922,-32.01722;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;38;-1242.208,404.3984;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;23;-1524.717,-305.0214;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;-1;None;84ad647ce0e708746becd7a4fb642667;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;30;-1502.244,661.1217;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-364.9538,205.0784;Inherit;False;Property;_ToxicityValue;ToxicityValue;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-137.2087,-537.5268;Inherit;False;Constant;_Color2;Color 2;4;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;24;-631.3685,-122.8097;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;25;-827.5524,285.9305;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;149.6295,-464.7141;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;17;45.11276,-157.3619;Inherit;False;Property;_hitValue;hitValue;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;124.6393,26.94361;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;13;542.9822,-111.8532;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1137.67,-118.7469;Float;False;True;-1;2;ASEMaterialInspector;0;4;Casco;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;33;0;32;0
WireConnection;35;0;33;0
WireConnection;21;0;20;0
WireConnection;29;0;28;0
WireConnection;29;1;26;0
WireConnection;29;2;26;4
WireConnection;39;0;2;0
WireConnection;39;1;28;0
WireConnection;39;2;35;0
WireConnection;22;0;21;0
WireConnection;38;0;2;0
WireConnection;38;1;28;0
WireConnection;38;2;40;0
WireConnection;30;0;39;0
WireConnection;30;1;29;0
WireConnection;30;2;35;0
WireConnection;24;0;38;0
WireConnection;24;1;23;0
WireConnection;24;2;22;0
WireConnection;25;0;2;0
WireConnection;25;1;30;0
WireConnection;25;2;40;0
WireConnection;16;0;15;1
WireConnection;16;1;15;2
WireConnection;16;2;15;3
WireConnection;16;3;2;4
WireConnection;5;0;25;0
WireConnection;5;1;24;0
WireConnection;5;2;12;0
WireConnection;13;0;5;0
WireConnection;13;1;16;0
WireConnection;13;2;17;0
WireConnection;0;0;13;0
ASEEND*/
//CHKSM=C245C0DD83D79CDFB80DAA1C0AC3CC9F518BFC20