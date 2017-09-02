// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

//Common Blend State Configurations
//Opaque:One Zero
//Alpha Blend: SrcAlpha OneMinusSrcAlpha
//Additive: One One
//Multiply: DstColor Zero
//Multiply 2x: DstColor SrcColor

//For translucent materials remember to change:
//ZWrite to Off
//Render Queue to Transparent

Shader "AUP/Standard" {
	Properties {
		[HDR]
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}
		[Toggle(ENABLE_BLEND_TEXTURE)] _EnableBlendTexture ("Enable Blend Texture", Float) = 0
		_MainTex2 ("MainTex2", 2D) = "white" {}
		_MainTexBlendFactor("Texture2Blend", Range(0, 1)) = 0

		_ScrollXSpeed("X Scroll Speed", Range(-10, 10)) = 0.0
		_ScrollYSpeed("Y Scroll Speed", Range(-10, 10)) = 0.0
		_Rotation("Rotation", Float) = 0.0

		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTestFunc("ZTest", Float) = 4
		[Header(Blend State)]
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend", Float) = 1 //"One"
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DestBlend", Float) = 0 //"Zero"
		[Space(20)]

		[Header(Other)]
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 1 //"On"
		_OffsetFalloff("OffsetFalloff", Float) = 0
		_Offset("Offset", Float) = 0
	}
 
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Blend[_SrcBlend][_DstBlend]
		ZTest[_ZTestFunc]
		ZWrite[_ZWrite]
		Cull[_Cull]

		Offset[_OffsetFalloff],[_Offset]

		Pass
		{
			CGPROGRAM
#pragma vertex Vert
#pragma fragment Frag
#pragma multi_compile_fog
#pragma shader_feature ENABLE_BLEND_TEXTURE

#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MainTex2;
			float _MainTexBlendFactor;
			float4 _MainTex_ST;
			float4 _Color;
			float _ScrollXSpeed;
			float _ScrollYSpeed;
			float _Rotation;

			struct V2F
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
				UNITY_FOG_COORDS(1)
			};

			float2 Rotate(float2 value, float2 pivot, float angle) {
				value.xy -= pivot;
				float s = sin(angle);
				float c = cos(angle);
				float2x2 rotationMatrix = float2x2(c, -s, s, c);
				rotationMatrix *= 0.5;
				rotationMatrix += 0.5;
				rotationMatrix = rotationMatrix * 2-1;
				value.xy = mul(value, rotationMatrix);
				value.xy += pivot;

				return value;
			}

			V2F Vert(appdata_full v)
			{
				V2F o;

				float4 vertex = UnityObjectToClipPos(v.vertex);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 normal = mul(unity_ObjectToWorld, float4(v.normal, 0)).xyz;
				float2 uv = v.texcoord;
				uv = TRANSFORM_TEX(uv, _MainTex);
				uv = Rotate(uv, float2(0.5, 0.5), _Rotation);

				o.vertex = vertex;
				o.normal = normal;
				o.color = v.color;
				o.uv = uv;

				UNITY_TRANSFER_FOG(o, o.vertex);

				return o;
			}

			float4 SampleMainTextures(float2 uv)
			{
				float4 color = tex2D(_MainTex, uv);
				#if defined(ENABLE_BLEND_TEXTURE)
				float4 color2 = tex2D(_MainTex2, uv);
				color = lerp(color, color2, _MainTexBlendFactor);
				#endif
				return color;
			}

			float4 Frag(V2F IN) : SV_Target
			{
				float2 uv = IN.uv;
				uv += float2(_ScrollXSpeed * _Time.y, _ScrollYSpeed * _Time.y);
				float4 color = SampleMainTextures(uv);
				color.rgb *= _Color;

				UNITY_APPLY_FOG(IN.fogCoord, color);

				return color;
			}
			ENDCG
		}
	}
Fallback "Diffuse"
}
 