Shader "KRZ" {
	Properties {
		_Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Color (RGBA)", 2D) = "white" {}
		_LightCutoff ("Light Cutoff", Range(0.0, 1.0)) = 0.2
		_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 1
		_AlphaMap ("Culling Mask", 2D) = "white" {}
		
		_GradientStrength ("Gradient Strength", Float) = 1
		_ColorLow("Color low", Color) = (1.0, 1.0, 1.0, 1.0)
		_ColorHigh("Color high", Color) = (1.0, 1.0, 1.0, 1.0)
		_ColorX("ColorX", Color) = (1.0, 1.0, 1.0, 1.0)
		_ColorY("ColorY", Color) = (1.0, 1.0, 1.0, 1.0)
		_yPosHigh("yHigh", Float) = 10
		_yPosLow("yLow", Float) = 1
		_EmissiveStrength("Emissive Strength", Float) = 1
	}
	
	SubShader {
   	 	Tags
		{ 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"CanUseSpriteAtlas"="True"
		}
    	LOD 300
 
		Cull Off
		Lighting Off
		Blend One OneMinusSrcAlpha
    
        
		CGPROGRAM
		#pragma surface surf KRZ fullforwardshadows alphatest:_Cutoff 
      
		sampler2D _MainTex;
		sampler2D _AlphaMap;
		float _LightCutoff;
		fixed4 _Color;
		
		struct SurfaceOutputKRZ {
			fixed3 Albedo;
			fixed3 Emission;
			fixed3 Normal;
			fixed Specular;
			fixed3 Alpha;
		};
        
		struct Input {
			float2 uv_MainTex;
			float2 uv_AlphaMap;
		};

		void surf (Input IN, inout SurfaceOutputKRZ o) {
	
			half3 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = tex.rgb;
			o.Alpha = tex2D(_AlphaMap, IN.uv_AlphaMap);;
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