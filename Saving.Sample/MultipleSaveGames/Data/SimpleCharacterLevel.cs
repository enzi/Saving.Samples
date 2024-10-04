// <copyright project="Saving.Sample" file="CharacterLevel.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using Unity.Entities;

namespace Saving.Sample
{
    public struct SimpleCharacterLevel : IComponentData, ISavable
    {
        public int Level;
    }
}