using UnityEditor;
using UnityEngine;

namespace Fizz6.Code
{
    [CustomEditor(typeof(EnumConfig), true)]
    public class EnumConfigEditor : FizzEditor
    {
        [MenuItem("CONTEXT/EnumConfig/Generate")]
        public static void Generate(MenuCommand command)
        {
            var enumConfig = command.context as EnumConfig;
            if (enumConfig == null)
            {
                return;
            }

            var namespaceNode = new NamespaceNode(enumConfig.NamespaceName);

            var enumNode = new EnumNode(enumConfig.EnumName)
            {
                Accessibility = enumConfig.EnumAccessibility
            };
            
            foreach (var enumValueName in enumConfig.EnumValueNames)
            {
                var enumValueNode = new EnumValueNode(enumValueName);
                enumNode.Add(enumValueNode);
            }
            
            namespaceNode.Add(enumNode);

            namespaceNode.Generate($"{Application.dataPath}/{enumConfig.Path}/{enumConfig.EnumName}.cs");
        }
    }
}