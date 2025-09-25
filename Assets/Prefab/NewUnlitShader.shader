Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _Blend ("Blend", Range(0,1)) = 0.0
        _Tex1 ("Night Skybox", Cube) = "" {}
        _Tex2 ("Day Skybox", Cube) = "" {}
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _Tex1;
            samplerCUBE _Tex2;
            float _Blend;

            struct v2f {
                float4 pos : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert(float4 vertex : POSITION)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.texcoord = normalize(vertex.xyz);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col1 = texCUBE(_Tex1, i.texcoord);
                fixed4 col2 = texCUBE(_Tex2, i.texcoord);
                return lerp(col1, col2, _Blend);
            }
            ENDCG
        }
    }
}
