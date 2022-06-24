// Ref: https://www.ronja-tutorials.com/post/012-fresnel/

Shader "Custom/FresnelShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        [HDR]_Emission ("Emission", Color) = (0,0,0)
        _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
        [PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.25, 4)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
        };

        half _Glossiness;
        half _Metallic;
        half3 _Emission;
        fixed4 _Color;
        float3 _FresnelColor;
        float _FresnelExponent;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            // get do product between the normal and the direction
            float fresnel = dot(IN.worldNormal, IN.viewDir);
            //clamp the value between 0 and 1
            fresnel = saturate(1 - fresnel);
            //multiply fresnel with colour
            float3 fresnelColor = fresnel * _FresnelColor;
            //raise the fresnel value to the exponents power to be able to adjust it
            fresnel = pow(fresnel, _FresnelExponent);
            //apply the fresnel value to the emission
            o.Emission = _Emission + fresnelColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
