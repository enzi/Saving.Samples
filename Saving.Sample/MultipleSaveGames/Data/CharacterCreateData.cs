// <copyright project="Saving.Sample" file="CharacterCreateData.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct CharacterCreateData : IComponentData
    {
        public SimpleCharacterName Name;
        public SimpleCharacterLevel Level;
    }
}