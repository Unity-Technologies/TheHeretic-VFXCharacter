Shader "Hidden/WriteAttributesTextures" {
	SubShader {
HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

struct a2v {
    float2 uv : TEXCOORD0;
#ifdef WRITE_TRANSFORM_ONLY
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
#endif
#ifdef WRITE_STATIC_ONLY
    float4 color : COLOR;
#endif
};

struct v2f {
    float4 vertex : SV_POSITION;
#ifdef WRITE_TRANSFORM_ONLY
    float4 position : TEXCOORD0;
    float4 normal : TEXCOORD1;
    float4 tangent : TEXCOORD2;
#endif
#ifdef WRITE_STATIC_ONLY
    float4 color : TEXCOORD0;
#endif
};

v2f Vert(a2v v) {
    v2f o;

    o.vertex = float4(v.uv.x * 2.f - 1.f, v.uv.y * -2.f + 1.f, 0.f, 1.f);
    
#ifdef WRITE_TRANSFORM_ONLY
    o.position = mul(GetRawUnityObjectToWorld(), v.vertex);
    o.normal = float4(normalize(mul(v.normal, (float3x3)GetRawUnityWorldToObject())), 1.f);
    o.tangent = float4(normalize(mul(v.tangent.xyz, (float3x3)GetRawUnityWorldToObject())), v.tangent.w);
#endif
    
#ifdef WRITE_STATIC_ONLY
    o.color = v.color;
#endif
    
    return o;
}

int _STRObjectID;

void Frag(v2f i
#ifdef WRITE_TRANSFORM_ONLY
    , out float4 position : SV_Target0
    , out float4 normal : SV_Target1
    , out float4 tangent : SV_Target2
#endif
#ifdef WRITE_STATIC_ONLY
    , out float4 color : SV_Target0
    , out float id : SV_Target1
#endif
)
{
#ifdef WRITE_TRANSFORM_ONLY
    position = i.position;
    normal = i.normal;
    tangent = i.tangent;
#endif

#ifdef WRITE_STATIC_ONLY
    color = i.color;
    id = _STRObjectID;
#endif
}
			
ENDHLSL

        Cull Off ZTest Always ZWrite Off ZClip Off
        
        Pass { Name "Static"
            HLSLPROGRAM
                #pragma multi_compile_local WRITE_STATIC_ONLY
                
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
        
        Pass { Name "TransformOnly"
            HLSLPROGRAM
                #pragma multi_compile_local WRITE_TRANSFORM_ONLY
                #pragma vertex Vert
                #pragma fragment Frag
            ENDHLSL
        }
	}
}
