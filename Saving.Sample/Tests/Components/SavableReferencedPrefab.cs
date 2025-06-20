// <copyright project="Saving.Sample" file="SavableReferencedPrefab.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using Unity.Entities;

namespace Saving.Sample
{
    public struct SavableReferencedPrefab : IComponentData, ISavable
    {
        public Entity ReferencedPrefab;
    }
}