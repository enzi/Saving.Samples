// <copyright project="Saving.Sample" file="CubeTag.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Saving.Sample
{
    [MaterialProperty("_BaseColor")]
    public struct ColoredCube : IComponentData
    {
        public float4 Color;
    }
}