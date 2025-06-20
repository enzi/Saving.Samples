// <copyright project="Saving.Sample" file="TestSharedComponent.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct TestSharedComponent : ISharedComponentData
    {
        public int GroupIndex;
    }
}