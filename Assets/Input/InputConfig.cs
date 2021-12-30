using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fizz6.Input
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Fizz6/Input/Config", order = 1)]
    public class InputConfig : ScriptableObject
    {
        [Serializable]
        public struct Item
        {
            [SerializeField]
            private InputType inputType;
            public InputType InputType => inputType;

            [SerializeField]
            private Source source;
            public Source Source => source;
        }
        
        [SerializeField] 
        private Item[] items;
        public IEnumerable<Item> Items => items;
    }
}
