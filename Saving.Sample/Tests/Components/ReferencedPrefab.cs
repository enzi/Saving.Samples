// <copyright project="Saving.Sample" file="ReferencedPrefab.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct ReferencedPrefabLookup : IComponentData
    {
        public Entity Prefab;
    }
}