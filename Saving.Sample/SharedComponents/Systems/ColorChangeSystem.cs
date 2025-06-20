// <copyright project="Saving.Sample" file="ColorChangeSystem.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Saving.Sample;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace SaveSystem_Samples.Saving.Sample.SharedComponents.Systems
{
    public partial struct ColorChangeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new ColorChangeJob().Schedule(state.Dependency);
        }

        [BurstCompile]
        private partial struct ColorChangeJob : IJobEntity
        {
            public void Execute(ref ColoredCube coloredCube, in TestSharedComponent sharedComponent)
            {
                coloredCube = sharedComponent.GroupIndex switch
                {
                    0 => new ColoredCube() { Color = new float4(1, 0, 0, 1) },
                    1 => new ColoredCube() { Color = new float4(0, 1, 0, 1) },
                    2 => new ColoredCube() { Color = new float4(0, 0, 1, 1) },
                    _ => coloredCube
                };
            }
        }
    }
}