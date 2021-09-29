using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Fizz6
{
    [CustomPropertyDrawer(typeof(ImplementationAttribute))]
    public class ImplementationAttributePropertyDrawer : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);

            var popupPosition = new Rect(
                position.x, 
                position.y, 
                position.width - position.height, 
                position.height
            );
            
            var buttonPosition = new Rect(
                position.x + (position.width - position.height), 
                position.y, 
                position.height,
                position.height
            );

            var managedReferenceFieldFullTypeName = property.GetManagedReferenceFieldFullTypeName();
            var type = TypeExt.GetType(managedReferenceFieldFullTypeName);
            
            var assignableTypes = type.GetAssignableTypes();
            var assignableTypeNames = assignableTypes
                .Select(assignableType => assignableType.FullName)
                .ToArray();

            var managedReferenceFullTypeName = property.GetManagedReferenceFullTypeName();
            var selectedIndex = Array.IndexOf(assignableTypeNames, managedReferenceFullTypeName);

            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                var index = EditorGUI.Popup(popupPosition, label.text, selectedIndex, assignableTypeNames);
                if (changeCheckScope.changed)
                {
                    var changeType = assignableTypes[index];
                    property.managedReferenceValue = index >= 0
                        ? Activator.CreateInstance(changeType) 
                        : null;
                    
                    Undo.RecordObject(property.serializedObject.targetObject, "Serialized Property Modification");
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            if (GUI.Button(buttonPosition, new GUIContent()))
            {
                SerializedPropertyWindow.For(property);
            }
        }
    }
}