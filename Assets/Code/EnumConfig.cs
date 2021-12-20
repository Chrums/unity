using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fizz6.Code
{
    [CreateAssetMenu(fileName = "EnumConfig", menuName = "Fizz6/Code/Enum Config", order = 1)]
    public class EnumConfig : ScriptableObject
    {
        [SerializeField]
        private string path;

        [SerializeField] 
        private string namespaceName;

        [SerializeField] 
        private Accessibility enumAccessibility;
        
        [SerializeField]
        private string enumName;
        
        [SerializeField]
        private List<string> enumValueNames;
        
#if UNITY_EDITOR
        public string Path => path;
        public string NamespaceName => namespaceName;
        public Accessibility EnumAccessibility => enumAccessibility;
        public string EnumName => enumName;
        public IReadOnlyList<string> EnumValueNames => enumValueNames;
#endif
    }
}