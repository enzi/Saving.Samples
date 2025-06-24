// <copyright project="Saving.Sample" file="NestedMenuViewModel.cs">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using BovineLabs.Core.UI;
using NZCore.Saving;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Saving.Sample
{
    public class NestedMenuViewModel: IViewModelBindingNotify<NestedMenuViewModel.Data>
    {
        private Data data;
        public ref Data Value => ref data;
        
        private List<ListElementClass> listElements = new();

        [CreateProperty]
        public List<ListElementClass> ListElements
        {
            get
            {
                if (!data.ListValid)
                {
                    return listElements;
                }

                var newListElements = new List<ListElementClass>();
                ref var list = ref data.ListRef;
                if (list.Length == 0)
                {
                    Debug.Log($"ListElements Clear copying {list.Length}");
                }

                foreach (var item in list)
                {
                    newListElements.Add(new ListElementClass()
                    {
                        Value1 = item.Value1,
                        Value2 = item.Value2,
                        Value3 = item.Value3,
                        ValueBetween = item.ValueBetween
                    });
                }

                listElements = newListElements;

                return listElements;
            }

            set => listElements = value;
        }

        [CreateProperty]
        public SavableComponent ComponentData
        {
            get => data.ComponentData;
            set => data.ComponentData = value;
        }

        public struct Data : IModelBindingNotify
        {
            private SavableComponent componentData;

            public bool ListValid => componentData.ListCreated && componentData.ListAccessor.IsCreated;
            public ref UnsafeList<ListElement> ListRef => ref componentData.ListAccessor;

            public SavableComponent ComponentData
            {
                get => componentData;
                set
                {
                    componentData = value;
                    this.Notify();
                }
            }

            public void ForceListRefresh()
            {
                this.Notify("ListElements");
            }
            
            public FunctionPointer<OnPropertyChangedDelegate> Notify { get; set; }
        }
        
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;
        public void OnPropertyChanged(in FixedString64Bytes property)
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property.ToString()));
        }
    }
}