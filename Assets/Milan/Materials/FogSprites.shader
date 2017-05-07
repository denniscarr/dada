Shader "Sprites/Fog"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
//
//      _DetailsTex ("Details Texture", 2D) = "white" {}
//      _ColorLight ("Light Color", Color) = (1,1,1,1)
//      _ColorShadow ("Shadow Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        Cull Off
        Lighting Off
        ZWrite On
//      ZTest Always
        Blend One OneMinusSrcAlpha // Premultiplied transparency
        Pass
        {
        CGPROGRAM
            #pragma vertex mySpriteVert
            #pragma fragment mySpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma multi_compile_fog // Fog
//            #include "UnitySprites.cginc"
            struct v2f_fog
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1) // Fog
                UNITY_VERTEX_OUTPUT_STEREO
            };
            v2f_fog mySpriteVert(appdata_t IN)
            {
                v2f_fog OUT;
                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
            #ifdef UNITY_INSTANCING_ENABLED
                IN.vertex.xy *= _Flip.xy;
            #endif
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
                UNITY_TRANSFER_FOG(OUT,OUT.vertex); // Fog
                return OUT;
            }
//          sampler2D _DetailsTex;
//          fixed4 _ColorLight;
//          fixed4 _ColorShadow;
//          fixed4 mySampleSpriteTexture (float2 uv)
//          {
//              fixed4 color = tex2D (_MainTex, uv);
//
//              fixed4 colorDetails = tex2D (_DetailsTex, uv);
//
//              color.rgb = colorDetails.rgb * _ColorLight + (1 - colorDetails.rgb) * _ColorShadow;
//
//          #if ETC1_EXTERNAL_ALPHA
//              fixed4 alpha = tex2D (_AlphaTex, uv);
//              color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
//          #endif
//
//              return color;
//          }
            fixed4 mySpriteFrag(v2f_fog IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                if(c.a < 0.1)
                    discard;
                UNITY_APPLY_FOG(IN.fogCoord, c); // Fog
                c.rgb *= c.a;
                return c;
            }
        ENDCG
        }
    }
}