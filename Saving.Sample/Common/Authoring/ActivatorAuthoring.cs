// <copyright project="Saving.Sample" file="ActivatorAuthoring.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using Unity.Entities;
using UnityEngine;

namespace Saving.Sample
{
    public class ActivatorAuthoring : MonoBehaviour
    {
        public SampleType SampleType;
        
        public class ActivatorAuthoringBaker : Baker<ActivatorAuthoring>
        {
            public override void Bake(ActivatorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                switch (authoring.SampleType)
                {
                    case SampleType.None:
                        break;
                    case SampleType.SavableSubScene:
                        AddComponent(entity, new ActivatorSavableSubScene());
                        AddComponent(entity, new ActivatorLoadSaveInterface());
                        break;
                    case SampleType.EnableableComponents:
                        AddComponent(entity, new ActivatorEnableableComponents());
                        AddComponent(entity, new ActivatorLoadSaveInterface());
                        break;
                    case SampleType.GlobalObjects:
                        AddComponent(entity, new ActivatorGlobalObjects());
                        AddComponent(entity, new ActivatorLoadSaveInterface());
                        break;
                    case SampleType.Migration:
                        AddComponent(entity, new ActivatorMigration());
                        AddComponent(entity, new ActivatorLoadSaveInterface());
                        break;
                    case SampleType.NestedData:
                        AddComponent(entity, new ActivatorNestedData());
                        AddComponent(entity, new ActivatorLoadSaveInterface());
                        break;
                    case SampleType.MultipleSaveGames:
                        AddComponent(entity, new ActivatorMultipleSaveGames());
                        break;
                    case SampleType.Game:
                        AddComponent(entity, new ActivatorGame());
                        break;
                    case SampleType.SharedComponents:
                        AddComponent(entity, new ActivatorSharedComponents());
                        AddComponent(entity, new ActivatorLoadSaveInterface());
                        break;
                    case SampleType.TestScene:
                        AddComponent(entity, new ActivatorTestScene());
                        AddComponent(entity, new ActivatorLoadSaveInterface());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public enum SampleType
    {
        None,
        SavableSubScene,
        EnableableComponents,
        GlobalObjects,
        Migration,
        NestedData,
        MultipleSaveGames,
        Game,
        SharedComponents,
        TestScene
    }
}