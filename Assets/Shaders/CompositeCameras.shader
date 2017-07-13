Shader "Hidden/CompositCameras"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainTex2("Texture2", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _MainTex2;

			float4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				float4 color2 = tex2D(_MainTex2, i.uv);

				float4 finalColor;
				finalColor.rgb = lerp(color.rgb, color2.rgb/color2.a, color2.a);
				finalColor.a = saturate(color.a+color2.a);

				return finalColor;
			}
			ENDCG
		}
	}
}
