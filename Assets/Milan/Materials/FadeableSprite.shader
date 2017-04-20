Shader "Sprites/FadeableSprites" {
	Properties {
		_Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Color (RGB) Alpha (A)", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 1
		_LightCutoff("Light Cutoff", Range(0.0, 1.0)) = 0
		_ScaleX ("Scale X", Float) = 1.0
     	 _ScaleY ("Scale Y", Float) = 1.0
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
		uniform float _ScaleX;
        uniform float _ScaleY;

		struct SurfaceOutputKRZ {
			fixed3 Albedo;
			fixed3 Emission;
			fixed3 Normal;
			fixed Specular;
			fixed3 Alpha;
		};

 		struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;
         };

         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;

            output.pos = mul(UNITY_MATRIX_P, 
              mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
              - float4(input.vertex.x, input.vertex.y, 0.0, 0.0)
              * float4(_ScaleX, _ScaleY, 1.0, 1.0));
 
            output.tex = input.tex;

            return output;
         }

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