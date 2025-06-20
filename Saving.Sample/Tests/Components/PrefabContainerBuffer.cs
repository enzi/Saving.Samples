// <copyright project="Saving.Sample" file="PrefabContainerBuffer.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct PrefabContainerBuffer : IBufferElementData
    {
        public Entity Prefab;
    }
}