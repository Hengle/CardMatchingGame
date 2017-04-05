// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'


Shader "AUP/Simple" {
 
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Illum ("Illum", Range (0, 1.0)) = .5
}
 
SubShader {
	//Tags {"RenderType"="Transparent" }
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert
 
sampler2D _MainTex;
fixed _Illum;
fixed4 _Color;
 
struct Input {
    float2 uv_MainTex;
//    float2 uv_Illum;
};
 
void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = c.rgb;
    o.Emission = c.rgb * _Illum;
}
ENDCG
}
 
Fallback "Diffuse"
}
 