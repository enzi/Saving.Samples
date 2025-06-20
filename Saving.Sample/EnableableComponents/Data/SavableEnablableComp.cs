// <copyright project="Saving.Sample" file="SavableEnablableComp.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore;
using Unity.Entities;

namespace Saving.Sample.Data
{
    public struct SavableEnableableComp : IComponentData, ISavable, IEnableableComponent { }
}