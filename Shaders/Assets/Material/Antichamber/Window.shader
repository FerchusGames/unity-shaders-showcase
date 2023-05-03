Shader "Antichamber/Window"
{
    Properties
    {
        [Header(Surface options)]
        [MainColor] _ColorTint ("Tint", Color) = (1,1,1,1)
        _StencilRef ("Stencil Reference", Int) = 0
    }
    
    SubShader
    {
        Tags{"RenderPipeline" = "UniversalPipeline"}
        Tags {"Queue" = "Transparent" "RenderType" = "Opaque"}
        
        ColorMask 0
        Zwrite off
        
        Stencil
        {
            Ref [_StencilRef]
            comp always
            pass replace
        }   
        
        Pass
        {
            Name "ForwardLit" // For debugging
            Tags{"LightMode" = "UniversalForward"}
            
            HLSLPROGRAM
            
            // Register our programmable stage functions
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "MyForwardLitPass.hlsl"
            
            ENDHLSL
        }
    }
}
