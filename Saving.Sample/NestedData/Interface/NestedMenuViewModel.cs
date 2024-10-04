// <copyright project="NZCore" file="NestedMenuViewModel.cs" version="0.1">
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
        
        private List<ListElementClass> listElements = new List<ListElementClass>();

        [CreateProperty]
        public List<ListElementClass> ListElements
        {
            get
            {
                if (data.ListValid)
                {
                    listElements.Clear();
                    ref var list = ref data.ListRef;
                    if (list.Length == 0)
                    {
                        Debug.Log($"ListElements Clear copying {list.Length}");
                    }

                    for (int i = 0; i < list.Length; i++)
                    {
                        var item = list[i];
                        listElements.Add(new ListElementClass()
                        {
                            Value1 = item.Value1,
                            Value2 = item.Value2,
                            Value3 = item.Value3,
                            ValueBetween = item.ValueBetween
                        });
                    }
                }

                return listElements;
            }

            set
            {
                listElements = value;  
            } 
        }

        [CreateProperty]
        public SavableComponent ComponentData
        {
            get => data.ComponentData;
            set
            {
                Debug.Log("Set ComponentData");
                data.ComponentData = value;
                data.Changed = true;
            }
        }

        [CreateProperty]
        public bool ListChanged
        {
            get
            {
                Debug.Log("Get ListChanged called");
                return false;
            }
            set
            {
                Debug.Log("Set ListChanged called");
                if (!value || !data.ListValid)
                {
                    return;
                }
                
                ref var list = ref data.ListRef;
                list.Clear();
                
                if (listElements.Count == 0)
                    Debug.Log($"ListChanged Clear copying {listElements.Count}");

                foreach (var element in listElements)
                {
                    list.Add(new ListElement()
                    {
                        Value1 = element.Value1,
                        Value2 = element.Value2,
                        Value3 = element.Value3,
                        ValueBetween = element.ValueBetween
                    });
                }
            }
        }

        public unsafe struct Data : IModelBindingNotify
        {
            private SavableComponent componentData;
            public bool Changed;
            

            public bool ListValid => componentData.List != null && componentData.List->IsCreated;
            public ref UnsafeList<ListElement> ListRef => ref * componentData.List;

            public SavableComponent ComponentData
            {
                get => componentData;
                set
                {
                    componentData = value;
                    this.Notify();
                }
            }
            
            public bool ListChanged
            {
                set => this.Notify();
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