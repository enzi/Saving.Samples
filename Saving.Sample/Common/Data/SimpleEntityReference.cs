// <copyright project="Saving.Sample" file="SimpleEntityReference.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using Unity.Entities;

namespace Saving.Sample
{
    public struct SimpleEntityReference : IComponentData, ISavable
    {
        public Entity Entity;
    }
}