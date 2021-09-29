using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Fizz6
{
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : Editor
    {
        private CustomEditorUtility.SerializedObjectEditor _serializedObjectEditor;
        
        private void OnEnable()
        {
            _serializedObjectEditor = new CustomEditorUtility.SerializedObjectEditor(serializedObject);
        }

        private void OnDisable()
        {
            _serializedObjectEditor = null;
        }
        
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            serializedObject.Update();
            _serializedObjectEditor.Render();
            serializedObject.ApplyModifiedProperties();
        }
    }
}