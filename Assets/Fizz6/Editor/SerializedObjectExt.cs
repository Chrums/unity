using System.Collections.Generic;
using UnityEditor;

namespace Fizz6.Editor
{
    public static class SerializedObjectExt
    {
        public static IEnumerable<SerializedProperty> GetSerializedProperties(this SerializedObject serializedObject)
        {
            var iterator = serializedObject.GetIterator();
            
            if (!iterator.NextVisible(enterChildren: true)) yield break;
            
            do
            {
                yield return iterator.Copy();
            }
            while (iterator.NextVisible(enterChildren: false));
        }
    }
}