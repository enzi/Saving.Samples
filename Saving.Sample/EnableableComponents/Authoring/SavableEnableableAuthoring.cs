// <copyright project="NZCore" file="SavableEnableableAuthoring.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Saving.Sample.Data;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class SavableEnableableAuthoring : MonoBehaviour
    {
        private class SavableEnableableComponentBaker : Baker<SavableEnableableAuthoring>
        {
            public override void Bake(SavableEnableableAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SavableEnableableComp()
                {
                    Value = 1
                });
            }
        }
    }
}