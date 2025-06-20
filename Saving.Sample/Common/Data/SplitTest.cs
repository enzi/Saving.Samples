// <copyright project="Saving.Sample" file="SplitTest.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct ToBeSplitTest : IComponentData
    {
        public int Value1;
        public float Value2;
        public int Value3;
        public float Value4;
    }
    
    public struct Split1TestComponent : IComponentData
    {
        public int Value1;
        public float Value2;
    }
    
    public struct Split2TestComponent : IComponentData
    {
        public int Value3;
        public float Value4;
    }
}