// <copyright project="Saving.Sample" file="TestSharedComponentAuthoring.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Saving.Sample;
using Unity.Entities;
using UnityEngine;

namespace SaveSystem_Samples.Saving.Sample.SharedComponents.Authoring
{
    public class TestSharedComponentAuthoring : MonoBehaviour
    {
        public int GroupId;
        
        private class TestSharedComponentAuthoring_Baker : Baker<TestSharedComponentAuthoring>
        {
            public override void Bake(TestSharedComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddSharedComponent(entity, new TestSharedComponent()
                {
                    GroupIndex = authoring.GroupId
                });
                
                // usually this would be in its own baker
                AddComponent<ColoredCube>(entity);
            }
        }
    }
}