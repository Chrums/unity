using System;
using UnityEngine;

namespace Fizz6.Input
{
    public abstract class Source : ScriptableObject
    {
        public float Value => Mathf.Clamp(Total, -1.0f, 1.0f);
        public bool IsDown => Value != 0.0f;
        
        protected abstract float Total { get; }
        
        public abstract bool IsPressed { get; }
        public event Action PressEvent;
        public abstract bool IsReleased { get; }
        public event Action ReleaseEvent;
        public abstract void Update();

        protected void Press() => PressEvent?.Invoke();
        protected void Release() => ReleaseEvent?.Invoke();
    }
}