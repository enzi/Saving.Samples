// <copyright project="Saving.Sample" file="SimpleCharacter.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.Saving;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class SimpleCharacter: MonoBehaviour
    {
        public int Level;
        public string Name;
        
        private class GameSceneReferenceAuthoringBaker : Baker<SimpleCharacter>
        {
            public override void Bake(SimpleCharacter authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new SimpleCharacterName()
                {
                    CharacterName = authoring.Name
                });
                AddComponent(entity, new SimpleCharacterLevel()
                {
                    Level = authoring.Level
                });
                
                // we want to inform the interface that a save state has been loaded or applied
                // so we can use ChangeFilterTracking and not brute force and set data all the time
                AddComponent<SaveStateLoaded>(entity);
                SetComponentEnabled<SaveStateLoaded>(entity, false);
            }
        }
    }
}