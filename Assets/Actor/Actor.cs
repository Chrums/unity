using UnityEngine;

namespace Fizz6.Actor
{
    public interface IActor
    {
        void Yield(IBehaviour behaviour);
    }

    public class Actor<T> : MonoBehaviour, IActor where T : IBehaviour
    {
        [SerializeReference, Implementation]
        private T[] behaviours;
        
        [SerializeField]
        private T _active;
        
        private void Awake()
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.Initialize(this);
            }
        }

        private void OnDestroy()
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.Dispose();
            }
        }

        private void Update()
        {
            if (_active == null) Query();
            if (_active == null) return;
            Yield();
            _active.Update();
        }

        private void Query()
        {
            foreach (var behaviour in behaviours)
            {
                if (!behaviour.Query()) continue;
                _active = behaviour;
                _active.Start();
                return;
            }
        }

        private void Yield()
        {
            foreach (var behaviour in behaviours)
            {
                if (_active.Equals(behaviour) || !behaviour.Query() || !_active.Yield(behaviour)) continue;
                _active.Stop();
                _active = behaviour;
                _active.Start();
                return;
            }
        }

        public void Yield(IBehaviour behaviour)
        {
            if (_active == null || _active.Equals(behaviour)) return;
            _active.Stop();
            _active = default;
        }
    }
    
    public class Actor : Actor<IBehaviour>
    {}
}