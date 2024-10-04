// <copyright project="NZCore" file="EnableableSampleSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Saving.Sample.Data;
using Unity.Entities;
using Unity.Transforms;

namespace Saving.Sample
{
    public partial struct EnableableSampleSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ActivatorEnableableComponents>();
        }

        public void OnUpdate(ref SystemState state)
        {
            new RotateSampleJob().Schedule();
        }

        [WithAll(typeof(SavableEnableableComp))]
        private partial struct RotateSampleJob : IJobEntity
        {
            public void Execute(ref LocalTransform transform)
            {
                transform = transform.RotateY(0.1f);
            }
        }
    }
}