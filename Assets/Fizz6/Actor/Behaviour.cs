using UnityEngine;

namespace Fizz6.Actor
{
    public interface IBehaviour
    {
        GameObject GameObject { get; }
        void Initialize(IActor actor);
        void Dispose();
        bool Query();
        bool Yield(IBehaviour behaviour);
        void Start();
        void Stop();
        void Update();
    }

    public abstract class Behaviour : IBehaviour
    {
        public GameObject GameObject => _actor.GameObject;

        private IActor _actor;

        public virtual void Initialize(IActor actor) => _actor = actor;
        public virtual void Dispose() {}
        public virtual bool Query() => false;
        public virtual bool Yield(IBehaviour behaviour) => false;
        public virtual void Start() {}
        public virtual void Stop() {}
        public virtual void Update() {}
        protected void Yield() => _actor.Yield(this);
    }
}