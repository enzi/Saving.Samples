// <copyright project="NZCore" file="SavableComponentAuthoring.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.Saving;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class SavableComponentAuthoring : MonoBehaviour
    {
        private class SavableComponentAuthoringBaker : Baker<SavableComponentAuthoring>
        {
            public override void Bake(SavableComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new SavableComponent()
                {
                    Value1 = 1,
                    Value2 = 2,
                    Value3 = 3,
                    //Value21 = 21
                });
            }
        }
    }
}