// This file contains the vertex and fragment functions for the forward lit pass
// This is the shader pass that computes visible colors for a material
// by reading material, light, shadow, etc. data

// This attributes struct receives data about the mesh we're currently rendering
// Data is automatically placed in fields according to their semantic

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

float4 _ColorTint;

struct Attributes
{
    float3 positionOS : POSITION; // Position in object space
};

// Output by the vertex function into the fragment function
// Fields will be transformed by the rasterizer
struct Interpolators
{
    float4 positionCS : SV_POSITION; 
};

Interpolators Vertex(Attributes input)
{
    Interpolators output;

    // Object space into world and clip space
    VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS);

    // Pass position and orientation data to the fragment function
    output.positionCS = positionInputs.positionCS;

    return output;
}

float4 Fragment(Interpolators input) : SV_TARGET
{
    return _ColorTint;
}