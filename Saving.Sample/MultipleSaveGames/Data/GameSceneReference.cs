// <copyright project="Saving.Sample" file="GameSceneReference.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using Unity.Entities.Serialization;

namespace Saving.Sample
{
    public struct GameSceneReference : IComponentData
    {
        public EntitySceneReference SceneReference;
        public Hash128 SceneHash;
    }
}