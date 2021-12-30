using Fizz6.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace Fizz6.Code.Editor
{
    [CustomEditor(typeof(EnumConfig), true)]
    public class EnumConfigEditor : FizzEditor
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            var monitor = Assets.Watch<EnumConfig>();
            monitor.AssetsModifiedProcessorAssetChangedEvent += Generate;
        }
        
        private static void Generate(EnumConfig enumConfig)
        {
            var path = $"{Application.dataPath}/{enumConfig.Path}/{enumConfig.EnumName}.cs";
            var assetsRelativePath = Assets.AbsoluteToAssetsRelativePath(path);
            
            try
            {
                AssetDatabase.StartAssetEditing();
            
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
            
                namespaceNode.Generate(path);
            
                var asset = AssetDatabase.LoadMainAssetAtPath(assetsRelativePath);
                EditorUtility.SetDirty(asset);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
                AssetDatabase.ImportAsset(assetsRelativePath);
            }
        }
    }
}