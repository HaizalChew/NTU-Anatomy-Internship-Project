// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StandardOccluded"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = ( 0.0, 0.0, 1.0, 1.0 )
        _Speed ("Speed", float) = 0.2
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OccludedColor("Occluded Color", Color) = (1,1,1,1)

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0,1)) = 0.1
    }
    SubShader {
   
        Pass
        {
            Tags { "Queue"="Geometry+1" }
            ZTest Greater
            ZWrite Off
 
            CGPROGRAM
            #pragma vertex vert            
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
 
            half4 _OccludedColor;
 
            float4 vert(float4 pos : POSITION) : SV_POSITION
            {
                float4 viewPos = UnityObjectToClipPos(pos);
                return viewPos;
            }
 
                half4 frag(float4 pos : SV_POSITION) : COLOR
            {
                return _OccludedColor;
            }
 
            ENDCG
        }
 
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1"}
        LOD 200
        ZWrite On
        ZTest LEqual
       
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
 
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
        };
 
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _Color2;
        float _Speed;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * lerp(_Color, _Color2, abs(fmod(_Time.a * _Speed, 2.0) - 1.0));
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG

        Pass{
            
            Cull Front

            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata{
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f{
                float4 position : SV_POSITION;
            };

            v2f vert(appdata v){
                v2f o;

                o.position = UnityObjectToClipPos(v.vertex +  normalize(v.normal) *  _OutlineThickness);
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET{
                return _OutlineColor;
            }

            // Attempt using Stencils

            ENDCG

        }
    }


    FallBack "Diffuse"
}