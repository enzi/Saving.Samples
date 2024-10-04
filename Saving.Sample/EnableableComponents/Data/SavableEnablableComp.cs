// <copyright project="NZCore" file="SavableEnablableComp.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using Unity.Entities;

namespace Saving.Sample.Data
{
    public struct SavableEnableableComp : IComponentData, ISavable, IEnableableComponent
    {
        public float Value;
    }
}