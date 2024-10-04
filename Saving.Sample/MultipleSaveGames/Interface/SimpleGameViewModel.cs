// <copyright project="Saving.Sample" file="SimpleGameViewModel.cs" version="0.1">
// Copyright © 2024 Thomas Enzenebner. All rights reserved.
// </copyright>

using System;
using BovineLabs.Core.UI;
using Unity.Burst;
using Unity.Collections;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Saving.Sample
{
    public class SimpleGameViewModel: IViewModelBindingNotify<SimpleGameViewModel.Data>
    {
        private Data data;
        public ref Data Value => ref data;
        
        [CreateProperty]
        public int CharacterLevel
        {
            get => data.CharacterLevel;
            set => data.CharacterLevel = value;
        }
        
        [CreateProperty]
        public string CharacterName
        {
            get => data.CharacterName.ToString();
            set => data.CharacterName = value;
        }
        
        [CreateProperty]
        public bool GainLevelButton
        {
            get => data.GainLevelButton;
            set => data.GainLevelButton = value;
        }

        public struct Data : IModelBindingNotify
        {
            public bool GainLevelButton;
            private int characterLevel;
            public FixedString64Bytes CharacterName;

            public int CharacterLevel
            {
                get => characterLevel;
                set
                {
                    if (characterLevel != value)
                    {
                        characterLevel = value;
                        this.Notify();
                    }
                }
            }
            
            public void Clear()
            {
                GainLevelButton = false;
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