// <copyright project="Saving.Sample" file="ManyTypesComponentAuthoring.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class ManyTypesComponentAuthoring : MonoBehaviour
    {
        public string textValue;
        
        private class ManyTypesComponentAuthoringBaker : Baker<ManyTypesComponentAuthoring>
        {
            public override void Bake(ManyTypesComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ManyTypesComponent
                {
                    FixedString32Bytes = authoring.textValue,
                    FixedString64Bytes = authoring.textValue,
                    FixedString128Bytes = authoring.textValue,
                    FixedString512Bytes = authoring.textValue,
                    FixedString4096Bytes = authoring.textValue
                });
            }
        }
    }
}