// <copyright project="Saving.Sample" file="GameSceneReferenceAuthoring.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEditor;
using UnityEngine;

namespace Saving.Sample
{
    public class GameSceneReferenceAuthoring : MonoBehaviour
    {
        public SceneAsset GameSubScene;
        
        private class GameSceneReferenceAuthoringBaker : Baker<GameSceneReferenceAuthoring>
        {
            public override void Bake(GameSceneReferenceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new GameSceneReference()
                {
                    SceneReference = new EntitySceneReference(authoring.GameSubScene),   
                    SceneHash = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(authoring.GameSubScene)),
                });
            }
        }
    }
}