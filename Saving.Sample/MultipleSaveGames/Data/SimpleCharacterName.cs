// <copyright project="Saving.Sample" file="SimpleCharacterName.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using Unity.Collections;
using Unity.Entities;

namespace Saving.Sample
{
    public struct SimpleCharacterName : IComponentData, ISavable
    {
        public FixedString64Bytes CharacterName;
    }
}