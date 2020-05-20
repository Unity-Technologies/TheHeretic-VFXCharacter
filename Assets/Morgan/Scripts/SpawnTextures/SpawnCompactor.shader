Shader "Hidden/SpawnCompactor" {
    SubShader {
        Pass {
            Cull Off ZTest Always ZWrite Off ZClip Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 vert(float2 uv : TEXCOORD0) : SV_Position {
                return float4(uv.x * 2.f - 1.f, uv.y * -2.f + 1.f, 0.f, 1.f);
            }

            float frag() : SV_Target {
                return 1;
            }
            ENDHLSL
        }
    }
}
