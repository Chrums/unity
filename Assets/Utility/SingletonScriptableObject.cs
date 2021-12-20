using UnityEngine;

namespace Fizz6
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance;
        public static T Instance => _instance == null
            ? _instance = Resources.Load<T>(typeof(T).Name)
            : _instance;

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}