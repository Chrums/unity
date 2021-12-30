namespace Fizz6.Actor
{
    public interface IBehaviour
    {
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
        private IActor _actor;

        public void Initialize(IActor actor) => _actor = actor;
        public void Dispose() {}
        protected void Yield() => _actor.Yield(this);
        public virtual bool Query() => false;
        public virtual bool Yield(IBehaviour behaviour) => false;
        public virtual void Start() {}
        public virtual void Stop() {}
        public virtual void Update() {}
    }
}