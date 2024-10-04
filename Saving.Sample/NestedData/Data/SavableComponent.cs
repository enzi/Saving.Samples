// <copyright project="NZCore" file="SavableComponent.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

namespace NZCore.Saving
{
    // V1
    // [ChunkSerializable]
    // public unsafe struct SavableComponent : IComponentData, ISavable
    // {
    //     public int Value1;
    //     public int Value2;
    //     public int Value21;
    //     public UnsafeList<ListElement>* List;
    //     public int Value3;
    // }
    //
    // public struct ListElement
    // {
    //     public int Value1;
    //     public int Value2;
    //     public int Value3;
    // }
    //
    // public struct ToBeChangedStruct
    // {
    //     public float3 FloatValue;
    // }
    
    // V2
    // [ChunkSerializable]
    // public unsafe struct SavableComponent : IComponentData, ISavable
    // {
    //     public int Value1;
    //     public int Value2;
    //     public int Value21;
    //     public ToBeChangedStruct ToBeChangedStruct;
    //     public UnsafeList<ListElement>* List;
    //     public int Value3;
    // }
    //
    // public struct ListElement
    // {
    //     public int Value1;
    //     public int Value2;
    //     public int ValueBetween;
    //     public int Value3;
    // }
    //
    // public struct ToBeChangedStruct
    // {
    //     public float3 FloatValue;
    // }
    
    
    // V3
    [ChunkSerializable]
    public unsafe struct SavableComponent : IComponentData, ISavable
    {
        public int Value1;
        public int Value2;
        public int Value21;
        public ToBeChangedStruct ToBeChangedStruct;
        public UnsafeList<ListElement>* List;
        public int Value3;
    }
    
    [Serializable]
    public struct ListElement
    {
        public int Value1;
        public int Value2;
        public int ValueBetween;
        public int Value3;
    }
    
    [Serializable]
    public struct ToBeChangedStruct
    {
        public float2 FloatValue;
    }

    public class SavableComponentClass
    {
        public int Value1;
        public int Value2;
        public int Value21;
    }
    
    [Serializable]
    public class ListElementClass
    {
        public int Value1;
        public int Value2;
        public int ValueBetween;
        public int Value3;
    }
}