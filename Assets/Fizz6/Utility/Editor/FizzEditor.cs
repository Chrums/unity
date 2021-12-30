using UnityEditor;

namespace Fizz6
{
    public abstract class FizzEditor : Editor
    {
        private SerializedObjectEditor _serializedObjectEditor;
        
        private void OnEnable()
        {
            _serializedObjectEditor = new SerializedObjectEditor(serializedObject);
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
