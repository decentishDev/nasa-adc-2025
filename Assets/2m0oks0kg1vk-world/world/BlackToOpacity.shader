Shader "Custom/BlackToOpacityWithLightFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightDirection ("Light Direction", Vector) = (0, 1, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        // Enable transparency blending
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _LightDirection;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = normalize(mul((float3x3)unity_WorldToObject, v.normal));
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                // Use brightness of the pixel to set opacity
                float alpha = texColor.r; // assuming grayscale where white (1) is opaque, black (0) is transparent

                // Calculate light fading effect based on angle between normal and light direction
                float3 lightDir = normalize(_LightDirection);
                float dotProduct = saturate(dot(i.worldNormal, lightDir));
                alpha *= dotProduct; // Modulate alpha based on light angle for shadow effect

                return float4(1, 1, 1, alpha); // White color with transparency affected by light direction
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
