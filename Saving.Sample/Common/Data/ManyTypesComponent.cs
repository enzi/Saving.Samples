// <copyright project="Saving.Sample" file="ManyTyesComponent.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using NZCore;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Saving.Sample
{
    public struct ManyTypesComponent : IComponentData, ISavable
    {
        public short ShortValue;
        public half HalfValue;
        public float FloatValue;
        public int IntValue;
        public uint UIntValue;
        public double DoubleValue;
        public long LongValue;
        public ulong ULongValue;
        public Entity EntityValue;
        public FixedString32Bytes FixedString32Bytes;
        public FixedString64Bytes FixedString64Bytes;
        public FixedString128Bytes FixedString128Bytes;
        public FixedString512Bytes FixedString512Bytes;
        public FixedString4096Bytes FixedString4096Bytes;
        public Guid GuidValue;
        public float2 Float2Value;
        public float3 Float3Value;
        public float4 Float4Value;
        public quaternion QuaternionValue;
        public float4x4 Float4X4Value;
        public SomeStruct SomeStructValue;
    }

    public struct SomeStruct
    {
        public int Value;
        public int Value2;
    }
}