Shader "FadeAbleSprite" {
	Properties {
		_Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Color (RGB) Alpha (A)", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 1
		_LightCutoff("Light Cutoff", Range(0.0, 1.0)) = 0
	}
	
	SubShader {
   	 	Tags {"IgnoreProjector"="True"  "Queue"="Transparent" "RenderType"="Transparent" }
    	LOD 300
 
		Cull Off
    
        
		CGPROGRAM
		#pragma surface surf KRZ fullforwardshadows alpha
      
		sampler2D _MainTex;
		fixed4 _Color;
		float _LightCutoff;

		struct SurfaceOutputKRZ {
			fixed3 Albedo;
			fixed3 Emission;
			fixed3 Normal;
			fixed Specular;
			fixed3 Alpha;
		};
        
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputKRZ o) {
	
			half3 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = tex.rgb;
			o.Alpha = tex2D (_MainTex, IN.uv_MainTex).a;
		}

		inline fixed4 LightingKRZ (SurfaceOutputKRZ s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{
			atten = step(_LightCutoff, atten) * atten;

			float4 c;
			c.rgb = (_LightColor0.rgb * s.Albedo) * (atten);
			c.a = s.Alpha;
			return c;
		}
	
		ENDCG
	}
	FallBack "VertexLit"
}