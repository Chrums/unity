using System;
using Fizz6.Actor;
using Fizz6.Input;
using UnityEngine;

namespace Ultima.Unit.Behaviours
{
    [Serializable]
    public class UnitMoveBehaviour : UnitBehaviour
    {
        private Source _horizontalSource;
        private Source _verticalSource;

        public override void Initialize(IActor actor)
        {
            base.Initialize(actor);
            _horizontalSource = InputManager.Instance.Sources[InputType.Horizontal];
            _verticalSource = InputManager.Instance.Sources[InputType.Vertical];
        }

        public override void Dispose()
        {
            base.Dispose();
            _horizontalSource = null;
            _verticalSource = null;
        }

        public override bool Query() => _horizontalSource.IsDown || _verticalSource.IsDown;
        public override void Update()
        {
            // base.Update();

            if (_horizontalSource.Value == 0 && _verticalSource.Value == 0)
            {
                Yield();
                return;
            }
            
            var position = GameObject.transform.position;
            GameObject.transform.position = new Vector3(
                position.x + _horizontalSource.Value, 
                position.y + _verticalSource.Value, 
                position.z
            );
        }
    }
}
