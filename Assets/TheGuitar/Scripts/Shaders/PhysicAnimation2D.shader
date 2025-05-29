Shader "Sago/PhysicAnimation2D"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _Stretch("Vertical Stretch", Float) = 1.0
        _Squash("Horizontal Squash", Float) = 1.0
        _Direction("Deform Direction", Vector) = (1,0,0,0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" "CanUseSpriteAtlas"="True"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float4    _Color;
            float     _Stretch;
            float     _Squash;
            float2 _Direction; // Dirección de movimiento normalizada (por ejemplo: (1, 0) o (0.7, 0.7))
            
            v2f vert(appdata_t v)
            {
                v2f    o;
                float3 pos = v.vertex.xyz;

                // Base directions
                float2 right = normalize(_Direction);  // Dirección de movimiento
                float2 up = float2(-right.y, right.x); // Perpendicular

                // Proyecta posición local sobre esas direcciones
                float squashProj = dot(pos.xy, right);
                float stretchProj = dot(pos.xy, up);

                // Escala esas proyecciones
                squashProj *= _Squash;
                stretchProj *= _Stretch;

                // Reconstruye la posición deformada
                pos.xy = squashProj * right + stretchProj * up;

                o.position = TransformObjectToHClip(pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }


            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Sprites/Default"
}