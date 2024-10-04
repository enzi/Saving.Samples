// <copyright project="NZCore" file="SamplesActivator.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using Unity.Entities;

namespace Saving.Sample
{
    public struct ActivatorLoadSaveInterface : IComponentData { }
    public struct ActivatorSavableSubScene : IComponentData { }
    public struct ActivatorEnableableComponents : IComponentData { }
    public struct ActivatorGlobalObjects : IComponentData { }
    public struct ActivatorMigration : IComponentData { }
    public struct ActivatorNestedData : IComponentData { }
    public struct ActivatorMultipleSaveGames : IComponentData { }
    public struct ActivatorGame : IComponentData { }
}