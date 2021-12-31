namespace Fizz6.Editor
{
    public abstract class FizzEditor : UnityEditor.Editor
    {
        private SerializedObjectEditor _serializedObjectEditor;
        
        protected virtual void OnEnable()
        {
            _serializedObjectEditor = new SerializedObjectEditor(serializedObject);
        }

        protected virtual void OnDisable()
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
