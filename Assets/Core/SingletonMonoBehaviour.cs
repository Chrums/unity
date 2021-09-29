using UnityEngine;

namespace Fizz6
{
    public class SingletonMonoBehaviour : MonoBehaviour
    {
        private static GameObject _singletonGameObject;

        protected static GameObject SingletonGameObject
        {
            get
            {
                if (_singletonGameObject == null)
                {
                    _singletonGameObject = new GameObject("Singleton")
                    {
                        hideFlags = HideFlags.DontSave
                    };
                }

                return _singletonGameObject;
            }
        }
    }
    
    public class SingletonMonoBehaviour<T> : SingletonMonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SingletonGameObject.AddComponent<T>();
                }

                return _instance;
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}