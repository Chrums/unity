using UnityEngine;

namespace Fizz6.Input
{
    [CreateAssetMenu(fileName = "KeySource", menuName = "Fizz6/Input/Sources/Key", order = 1)]
    public class KeySource : Source
    {
        [SerializeField]
        private KeyCode keyCode;
        
        [SerializeField]
        private float value;

        protected override float Total => _isDown ? value : 0.0f;
        public override bool IsPressed => _isPressed;
        public override bool IsReleased => _isReleased;

        private bool _isPressed = false;
        private bool _isReleased = false;
        private bool _isDown = false;

        public override void Update()
        {
            _isPressed = false;
            _isReleased = false;
            
            if (UnityEngine.Input.GetKeyDown(keyCode))
            {
                _isPressed = true;
                _isDown = true;
                Press();
            }

            if (UnityEngine.Input.GetKeyUp(keyCode))
            {
                _isReleased = true;
                _isDown = false;
                Release();
            }
        }
    }
}