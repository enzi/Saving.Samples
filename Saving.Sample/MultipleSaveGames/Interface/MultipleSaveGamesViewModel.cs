// <copyright project="Saving.Sample" file="MultipleSaveGamesViewModel.cs" version="0.1">
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
    public class MultipleSaveGamesViewModel : IViewModelBindingNotify<MultipleSaveGamesViewModel.Data>
    {
        private Data data;
        public ref Data Value => ref data;
        
        
        [CreateProperty]
        public bool CreateCharacter
        {
            get => data.CreateCharacter;
            set => data.CreateCharacter = value;
        }
        
        [CreateProperty]
        public string CharacterName
        {
            get => data.CharacterName.ToString();
            set => data.CharacterName = value;
        }
        

        public struct Data : IModelBindingNotify
        {
            public bool CreateCharacter;
            public FixedString64Bytes CharacterName;
            
            public void Clear()
            {
                CreateCharacter = false;
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