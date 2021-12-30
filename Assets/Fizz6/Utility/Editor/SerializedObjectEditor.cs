using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Fizz6
{
    public class SerializedObjectEditor
    {
        private const string ScriptSerializedPropertyName = "m_Script";
        
        private readonly SerializedObject _serializedObject;
        private readonly Dictionary<string, ReorderableList> _lists = new Dictionary<string, ReorderableList>();
            
        public SerializedObjectEditor(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
            var serializedProperties = _serializedObject.GetSerializedProperties();
                
            foreach (var serializedProperty in serializedProperties)
            {
                var type = serializedProperty?.GetValue()?.GetType();
                if (type == null || !(type.IsArray || type.GetInterfaces().Contains(typeof(IList)))) continue;
                
                var copy = serializedProperty.Copy();
                var reorderableList = new ReorderableList(copy);

                _lists.Add(copy.name, reorderableList);
            }
        }

        public void Render()
        {
            var serializedProperties = _serializedObject.GetSerializedProperties();
            foreach (var serializedProperty in serializedProperties)
            {
                if (_lists.TryGetValue(serializedProperty.name, out var list))
                {
                    list.Render();
                }
                else
                {
                    if (serializedProperty.name == ScriptSerializedPropertyName)
                    {
                        using (new EditorGUI.DisabledScope(true))
                        {
                            EditorGUILayout.PropertyField(serializedProperty);
                            EditorGUILayout.Space();
                        }

                        continue;
                    }
                    
                    EditorGUILayout.PropertyField(serializedProperty);
                }
            }
        }
    }
}