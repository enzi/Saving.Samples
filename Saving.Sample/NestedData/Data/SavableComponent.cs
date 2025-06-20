// <copyright project="Saving.Sample" file="SavableComponent.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Unity.Collections;
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
    public unsafe struct SavableComponent : IComponentData, ISavable, IDisposable
    {
        public int Value1;
        public int Value2;
        public int Value21;
        public ToBeChangedStruct ToBeChangedStruct;
        private UnsafeList<ListElement>* List;
        public int Value3;
        
        // Keeping the pointer private and using a referenced accessor makes using the list much less error-prone
        // as in, it avoids accidentally making local changes. The component itself needs ref access too,
        // otherwise Adds to the list and therefore changes to the Length of it wouldn't be written back
        // to the component
        public bool ListCreated => List != null && List->IsCreated;
        public ref UnsafeList<ListElement> ListAccessor => ref *List;

        public void Init()
        {
            List = UnsafeList<ListElement>.Create(0, Allocator.Persistent);
        }

        public void Dispose()
        {
            if (List != null)
            {
                UnsafeList<ListElement>.Destroy(List);
            }
        }
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