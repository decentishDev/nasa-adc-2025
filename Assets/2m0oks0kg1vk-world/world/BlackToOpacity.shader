Shader "Custom/LightBasedTextureBlend_WithOpacity"
{
    Properties
    {
        _MainTex ("Lit Side Texture", 2D) = "white" {}
        _ShadowTex ("Shadow Side Texture", 2D) = "black" {}
        _LightDirection ("Light Direction", Vector) = (0, 1, 0, 0)
        _Opacity ("Opacity", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _ShadowTex;
            float4 _LightDirection;
            float _Opacity;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normalize the light direction
                float3 lightDir = normalize(_LightDirection.xyz);

                // Calculate how much the surface is facing the light
                float facing = max(0, dot(i.worldNormal, lightDir));

                // Sample both textures
                fixed4 texColorLit = tex2D(_MainTex, i.uv);
                fixed4 texColorShadow = tex2D(_ShadowTex, i.uv);

                // Blend textures based on facing ratio
                fixed4 finalColor = lerp(texColorShadow, texColorLit, facing);

                // Apply global opacity
                finalColor.a *= _Opacity;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Unlit"
}
