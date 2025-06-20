using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class PrefabContainerAuthoring : MonoBehaviour
    {
        public List<GameObject> Prefabs;

        private class PrefabContainerAuthoringBaker : Baker<PrefabContainerAuthoring>
        {
            public override void Bake(PrefabContainerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var buffer = AddBuffer<PrefabContainerBuffer>(entity);

                foreach (var prefab in authoring.Prefabs)
                {
                    buffer.Add(new PrefabContainerBuffer()
                    {
                        Prefab = GetEntity(prefab, TransformUsageFlags.Dynamic)
                    });
                }
            }
        }
    }
}