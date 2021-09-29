using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Fizz6
{
    public static class CustomEditorUtility
    {
        public class ReorderableList
        {
            private readonly UnityEditorInternal.ReorderableList _reorderableList;
            
            public ReorderableList(SerializedProperty serializedProperty)
            {
                if (!serializedProperty.isArray) return;
                
                var copy = serializedProperty.Copy();
                _reorderableList = new UnityEditorInternal.ReorderableList(copy.serializedObject, copy, true, true, true, true);
                
                _reorderableList.drawHeaderCallback =
                    rect =>
                    {
                        EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(copy.name));
                    };
                
                _reorderableList.drawElementCallback =
                    (rect, index, isActive, isFocused) =>
                    {
                        var arrayElementSerializedProperty = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(rect, arrayElementSerializedProperty); 
                    };
                
                _reorderableList.onAddCallback =
                    _ =>
                    {
                        copy.arraySize++;
                        var index = copy.arraySize - 1;
                        serializedProperty.serializedObject.ApplyModifiedProperties();
                        
                        var arrayElementSerializedProperty = copy.GetArrayElementAtIndex(index);
                        if (arrayElementSerializedProperty.propertyType != SerializedPropertyType.Generic) return;
                        var childType = arrayElementSerializedProperty
                            .GetValue()
                            .GetType();
                        
                        if (copy.GetValue() is IList list) list[index] = Activator.CreateInstance(childType);
                    };
            }

            public void Render()
            {
                _reorderableList.DoLayoutList();
            }
        }

        public class SerializedObjectEditor
        {
            private readonly SerializedObject _serializedObject;
            private readonly Dictionary<string, ReorderableList> _lists = new Dictionary<string, ReorderableList>();
            
            public SerializedObjectEditor(SerializedObject serializedObject)
            {
                _serializedObject = serializedObject;
                var serializedProperties = _serializedObject.GetSerializedProperties();
                
                foreach (var serializedProperty in serializedProperties)
                {
                    if (!serializedProperty.isArray) continue;
                
                    var copy = serializedProperty.Copy();
                    var reorderableList = new ReorderableList(copy);

                    _lists.Add(copy.name, reorderableList);
                }
            }

            public void Render()
            {
                using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
                {
                    _serializedObject.Update();
                    
                    var serializedProperties = _serializedObject.GetSerializedProperties();
                    foreach (var serializedProperty in serializedProperties)
                    {
                        if (serializedProperty.isArray && _lists.TryGetValue(serializedProperty.name, out var list))
                        {
                            list.Render();
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedProperty);
                        }
                    }
                        
                    if (!changeCheckScope.changed) return;
                        
                    Undo.RecordObject(_serializedObject.targetObject, "Serialized Property Modification");
                    EditorUtility.SetDirty(_serializedObject.targetObject);
                    _serializedObject.ApplyModifiedProperties();
                }
            }
        }
        
        public class SerializedPropertyEditor
        {
            private readonly SerializedProperty _serializedProperty;
            private readonly Dictionary<string, ReorderableList> _lists = new Dictionary<string, ReorderableList>();
            
            public SerializedPropertyEditor(SerializedProperty serializedProperty)
            {
                _serializedProperty = serializedProperty;
                var serializedProperties = _serializedProperty.GetChildren();
                
                foreach (var childSerializedProperty in serializedProperties)
                {
                    if (!childSerializedProperty.isArray) continue;
                
                    var copy = childSerializedProperty.Copy();
                    var reorderableList = new ReorderableList(copy);

                    _lists.Add(copy.name, reorderableList);
                }
            }

            public void Render()
            {
                using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
                {
                    _serializedProperty.serializedObject.Update();
                    
                    var serializedProperties = _serializedProperty.GetChildren();
                    foreach (var serializedProperty in serializedProperties)
                    {
                        if (serializedProperty.isArray && _lists.TryGetValue(serializedProperty.name, out var list))
                        {
                            list.Render();
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedProperty);
                        }
                    }
                    
                    if (!changeCheckScope.changed) return;
                    
                    Undo.RecordObject(_serializedProperty.serializedObject.targetObject, "Serialized Property Modification");
                    EditorUtility.SetDirty(_serializedProperty.serializedObject.targetObject);
                    _serializedProperty.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}