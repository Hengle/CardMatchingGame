  Shader "AUP/Scrolling" {
        Properties {
			_MainTint ("Diffuse Tint", Color) = (1,1,1,1)
            _MainTex ("Base (RGB)", 2D) = "white" {}
			_ScrollXSpeed ("X Scroll Speed", Range(-10, 10)) = 2.0
			_ScrollYSpeed ("Y Scroll Speed", Range(-10, 10)) = 2.0
			_Illum ("Illum", Range (0, 1.0)) = .5
        }
        SubShader {
            Tags { "Queue"="Transparent" "RenderType"="Transparent" }
            LOD 200
           
            CGPROGRAM
            #pragma surface surf Lambert alpha
     
			fixed4 _MainTint;
			fixed _ScrollXSpeed;
			fixed _ScrollYSpeed;
			fixed _Illum;
            sampler2D _MainTex;
     
            struct Input {
                float2 uv_MainTex;
            };

            void surf (Input IN, inout SurfaceOutput o) {  
				fixed2 scrolledUV = IN.uv_MainTex;
				fixed xScrollValue = _ScrollXSpeed * _Time;
				fixed yScrollValue = _ScrollYSpeed * _Time;
				scrolledUV += fixed2 (xScrollValue, yScrollValue);
                half4 c = tex2D (_MainTex, scrolledUV);
                o.Albedo = c.rgb * _MainTint;
                o.Alpha = c.a;
                o.Emission = c.rgb * _Illum;
            }
            ENDCG
        }
        FallBack "Diffuse"
    }
