// <copyright project="NZCore" file="NestedDataSystem.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using NZCore.Saving;
using NZCore.UIToolkit;
using Saving.Sample;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Saving.Sample
{
    public unsafe partial struct NestedDataSystem : ISystem, ISystemStartStop
    {
        private UIHelper<NestedMenuViewModel, NestedMenuViewModel.Data> ui;
        private UnsafeList<ListElement>* list;
        private Entity globalObjectEntity;

        private EntityQuery saveStateLoadedQuery;
        
        public void OnCreate(ref SystemState state)
        {
            saveStateLoadedQuery = SystemAPI.QueryBuilder()
                .WithAll<SavableComponent>()
                .WithPresent<SaveStateLoaded>()
                .Build();
            
            saveStateLoadedQuery.AddChangedVersionFilter(ComponentType.ReadOnly<SaveStateLoaded>());
            
            var archetype = state.EntityManager.CreateArchetype(stackalloc ComponentType[]
            {
                // indicates that we want to save global object data
                ComponentType.ReadOnly<SavableObject>(), 
                // state for the system to automatically apply save data when loaded
                ComponentType.ReadOnly<SaveStateLoaded>(),
                // our actual data to save
                ComponentType.ReadOnly<SavableComponent>() 
            });
            
            globalObjectEntity = state.EntityManager.CreateEntity(archetype);
            state.EntityManager.SetComponentEnabled<SaveStateLoaded>(globalObjectEntity, false);
            
            state.EntityManager.SetComponentData(globalObjectEntity, new SavableObject()
            {
                SaveId = 102
            });

            list = UnsafeList<ListElement>.Create(0, Allocator.Persistent);
            
            list->Add(new ListElement()
            {
                Value1 = 1,
                Value2 = 2,
                Value3 = 3,
                ValueBetween = 4
            });
            
            list->Add(new ListElement()
            {
                Value1 = 5,
                Value2 = 6,
                Value3 = 7,
                ValueBetween = 8
            });
            
            SavableComponent comp = new SavableComponent()
            {
                Value1 = 1,
                Value2 = 2,
                Value3 = 3,
                Value21 = 4,
                ToBeChangedStruct = new ToBeChangedStruct() { FloatValue = new float2(3,3)},
                List = list
            };

            if (list == null)
            {
                Debug.Log("list is null");
            }

            if (comp.List == null)
            {
                Debug.Log("comp list is null");
            }

            //Debug.Log("setup list");
            state.EntityManager.SetComponentData(globalObjectEntity, comp);
            
            state.RequireForUpdate<ActivatorNestedData>();
            state.RequireForUpdate<UIAssetsLoaded>();
        }

        public void OnDestroy(ref SystemState state)
        {
            list->Dispose();
        }

        public void OnStartRunning(ref SystemState state)
        {
            ui = new UIHelper<NestedMenuViewModel, NestedMenuViewModel.Data>("nested-menu", "nested-menu");
            var element = ui.LoadPanel();

            var compData = SystemAPI.GetComponent<SavableComponent>(globalObjectEntity);

            ui.Model.ComponentData = compData;

            var listView = element.Q<ListView>();

            var assets = SystemAPI.ManagedAPI.GetSingleton<UIAssetsSingleton>();

            listView.makeItem += () =>
            {
                var item = assets.VisualTreeAssets["nested-list-element"].CloneTree();

                return item;
            };

            var system = this;

            var uiHelper = ui;
            listView.bindItem += (visualElement, i) =>
            {
                var data = (ListElementClass) listView.itemsSource[i];
                
                var value1Field = visualElement.Q<IntegerField>("value1");
                var value2Field = visualElement.Q<IntegerField>("value2");
                var value3Field = visualElement.Q<IntegerField>("value3");
                var value4Field = visualElement.Q<IntegerField>("value-between");
                
                value1Field.value = data.Value1;
                value2Field.value = data.Value2;
                value3Field.value = data.Value3;
                value4Field.value = data.ValueBetween;
                
                value1Field.RegisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback1, new ValueChangeContext() { Index = i, ListView = listView, Ui = uiHelper });
                value2Field.RegisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback2, new ValueChangeContext() { Index = i, ListView = listView, Ui = uiHelper });
                value3Field.RegisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback3, new ValueChangeContext() { Index = i, ListView = listView, Ui = uiHelper });
                value4Field.RegisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback4, new ValueChangeContext() { Index = i, ListView = listView, Ui = uiHelper });
            };

            listView.unbindItem += (visualElement, _) =>
            {
                var value1Field = visualElement.Q<IntegerField>("value1");
                value1Field.UnregisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback1);
                var value2Field = visualElement.Q<IntegerField>("value2");
                value2Field.UnregisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback2);
                var value3Field = visualElement.Q<IntegerField>("value3");
                value3Field.UnregisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback3);
                var value4Field = visualElement.Q<IntegerField>("value-between");
                value4Field.UnregisterCallback<ChangeEvent<int>, ValueChangeContext>(system.Callback4);
            };
        }

        private void Callback1(ChangeEvent<int> evt, ValueChangeContext context)
        {
            if (context.ListView.itemsSource[context.Index] is ListElementClass element)
                element.Value1 = evt.newValue;
            
            context.Ui.Model.ListRef.ElementAt(context.Index).Value1 = evt.newValue;
        }
        
        private void Callback2(ChangeEvent<int> evt, ValueChangeContext context)
        {
            if (context.ListView.itemsSource[context.Index] is ListElementClass element) 
                element.Value2 = evt.newValue;
            
            context.Ui.Model.ListRef.ElementAt(context.Index).Value2 = evt.newValue;
        }
        
        private void Callback3(ChangeEvent<int> evt, ValueChangeContext context)
        {
            if (context.ListView.itemsSource[context.Index] is ListElementClass element) 
                element.Value3 = evt.newValue;
            
            context.Ui.Model.ListRef.ElementAt(context.Index).Value3 = evt.newValue;
        }
        
        private void Callback4(ChangeEvent<int> evt, ValueChangeContext context)
        {
            if (context.ListView.itemsSource[context.Index] is ListElementClass element) 
                element.ValueBetween = evt.newValue;
            
            context.Ui.Model.ListRef.ElementAt(context.Index).ValueBetween = evt.newValue;
        }

        public void OnStopRunning(ref SystemState state)
        {
            ui.Unload();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            if (ui.Model.Changed)
            {
                SystemAPI.SetComponent(globalObjectEntity, ui.Model.ComponentData);
            }

            if (!saveStateLoadedQuery.IsEmpty)
            {
                var compData = SystemAPI.GetComponent<SavableComponent>(globalObjectEntity);
                ui.Model.ComponentData = compData;
            }
        }
    }

    public struct ValueChangeContext
    {
        public ListView ListView;
        public int Index;
        public UIHelper<NestedMenuViewModel, NestedMenuViewModel.Data> Ui;
    }
}