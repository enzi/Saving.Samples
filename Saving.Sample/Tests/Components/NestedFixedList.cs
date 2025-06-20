// <copyright project="Saving.Sample" file="NestedFixedList.cs">
// Copyright © 2025 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using Unity.Entities;

namespace Saving.Sample
{
    public struct NestedFixedList : ISavable
    {
        public FixedEntityList WorkedOnBy;
        public int NotYetArrivedWorkers, NotYetContinuedWorkers;
        public Entity TargetTask;
    }

    public struct FixedEntityList 
    {
        public Entity Element0;
        public Entity Element1;
        public Entity Element2;
        public Entity Element3;
        public Entity Element4;
        public int Count;
    }
}