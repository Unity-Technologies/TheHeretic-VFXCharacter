Shader "Hidden/VFXNullShader" {
    SubShader {
        Tags { "LightMode" = "Ignore" }
        Pass {
            HLSLPROGRAM
                #pragma multi_compile_local WRITE_STATIC_ONLY
                
                #pragma vertex Vert
                #pragma fragment Frag

                struct a2v {
                    float2 uv : TEXCOORD0;
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float4 color : COLOR;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float4 position : TEXCOORD0;
                    float4 normal : TEXCOORD1;
                    float4 tangent : TEXCOORD2;
                    float4 color : TEXCOORD3;
                };
                
                v2f Vert(a2v v) {
                    v2f o;
                    o.vertex = float4(v.uv.x * 0.00000001f, 0, 0.f, 1.f);
                    o.position = v.vertex;
                    o.normal = float4(v.normal, 1.f);
                    o.tangent = v.tangent;
                    o.color = v.color;
                    return o;
                }
                void Frag(v2f i) {}
            ENDHLSL
        }
    }
}
