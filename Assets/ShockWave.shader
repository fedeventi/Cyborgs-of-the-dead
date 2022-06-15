// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/ShockWave"
{
	Properties
	{
		_Deformation("Deformation", Range( 0 , 1)) = 0
		_cycle("cycle", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Deformation;
		uniform float _cycle;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float temp_output_69_0 = (1.0 + (_Deformation - 0.0) * (0.0 - 1.0) / (1.0 - 0.0));
			float4 lerpResult85 = lerp( ase_grabScreenPosNorm , ( ( ase_grabScreenPosNorm * temp_output_69_0 ) + ( ( 1.0 - temp_output_69_0 ) * 0.5 ) ) , ( (0.0 + (sin( ( ( distance( float3( i.uv_texcoord ,  0.0 ) , float3(0.5,0.5,0) ) * _cycle ) + _Time.y ) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) * 1.0 ));
			float4 screenColor7 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,lerpResult85.xy);
			o.Emission = screenColor7.rgb;
			float temp_output_142_0 = saturate( ceil( ( ( ( distance( ( i.uv_texcoord + float2( -0.5,-0.5 ) ) , float2( 0,0 ) ) * 1.72 ) + -0.75 ) * -1.0 ) ) );
			o.Alpha = temp_output_142_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
156;257;1109;592;-355.408;93.05643;1.267227;False;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;93;-1451.033,-958.7698;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;127;-1219.41,168.6867;Inherit;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;0.5,0.5,0;0.5,0.5,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;123;-804.0205,244.6891;Inherit;False;Property;_cycle;cycle;2;0;Create;True;0;0;False;0;0;36.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;125;-916.757,50.458;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;122;-742.8312,409.2402;Inherit;False;Constant;_speed;speed;3;0;Create;True;0;0;False;0;1;3.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;148;-473.6543,378.018;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-335.2586,114.9854;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;149;-242.3433,310.7364;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-2045.982,-103.3665;Inherit;False;Property;_Deformation;Deformation;0;0;Create;True;0;0;False;0;0;0.1529412;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;120;-33.84282,245.3217;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-920.3665,497.823;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;-0.5,-0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DistanceOpNode;137;-714.3134,661.5309;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;130;331.2939,185.2246;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;69;-1685.622,-190.4345;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;541.8491,405.6654;Inherit;False;Constant;_Float4;Float 4;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-516.0737,717.5924;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.72;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;62;-1175.615,-234.8487;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;3;-1923.043,-579.2059;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;784.2317,110.5451;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1227.795,-92.03001;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1001.194,-224.7797;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;150;894.8607,4.316082;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;138;-304.5979,717.5929;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1459.98,-374.8498;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-9.571589,735.1097;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-795.0138,-370.4535;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;136;-382.3079,42.88957;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;147;184.1885,725.7478;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;85;-335.878,-184.0135;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1342.253,-683.0822;Inherit;False;Constant;_Float6;Float 6;5;0;Create;True;0;0;False;0;1.1;1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;118;512.1691,-250.5147;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;95;-976.4063,-702.8036;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-1055.297,-924.6851;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-724.7815,-607.814;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;7;-85.59145,-242.4051;Inherit;False;Global;_GrabScreen0;Grab Screen 0;1;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;971.149,424.1599;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;154;830.991,613.2885;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;142;401.8636,639.5956;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;488.1953,780.465;Inherit;False;Property;_opacity;opacity;3;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-463.779,-662.5528;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;88;-199.4674,-539.002;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1543.851,-161.0802;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/ShockWave;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;125;0;93;0
WireConnection;125;1;127;0
WireConnection;148;0;122;0
WireConnection;124;0;125;0
WireConnection;124;1;123;0
WireConnection;149;0;124;0
WireConnection;149;1;148;0
WireConnection;120;0;149;0
WireConnection;139;0;93;0
WireConnection;137;0;139;0
WireConnection;130;0;120;0
WireConnection;69;0;61;0
WireConnection;140;0;137;0
WireConnection;62;0;69;0
WireConnection;131;0;130;0
WireConnection;131;1;135;0
WireConnection;56;0;62;0
WireConnection;56;1;57;0
WireConnection;150;0;131;0
WireConnection;138;0;140;0
WireConnection;65;0;3;0
WireConnection;65;1;69;0
WireConnection;143;0;138;0
WireConnection;55;0;65;0
WireConnection;55;1;56;0
WireConnection;136;0;150;0
WireConnection;147;0;143;0
WireConnection;85;0;3;0
WireConnection;85;1;55;0
WireConnection;85;2;136;0
WireConnection;118;0;7;0
WireConnection;118;1;88;0
WireConnection;118;2;88;4
WireConnection;95;0;98;0
WireConnection;94;0;93;0
WireConnection;94;1;98;0
WireConnection;96;0;95;0
WireConnection;7;0;85;0
WireConnection;152;0;142;0
WireConnection;152;1;154;0
WireConnection;154;0;153;0
WireConnection;142;0;147;0
WireConnection;97;0;94;0
WireConnection;97;1;96;0
WireConnection;88;1;97;0
WireConnection;0;2;7;0
WireConnection;0;9;142;0
ASEEND*/
//CHKSM=22DBBE5FB6955E05A51BD044BCF555BB21185506