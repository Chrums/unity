using System.Linq;
using UnityEngine;

namespace Fizz6.Input
{
    [CreateAssetMenu(fileName = "CompositeSource", menuName = "Fizz6/Input/Sources/Composite", order = 1)]
    public class CompositeSource : Source
    {
        [SerializeField]
        private Source[] sources;

        protected override float Total => 
            sources.Aggregate(0.0f, (total, source) => total + source.Value);

        public override bool IsPressed =>
            sources.Aggregate(false, (isPressed, source) => isPressed || source.IsPressed);

        public override bool IsReleased =>
            sources.Aggregate(false, (isReleased, source) => isReleased || source.IsReleased);

        private bool _isDown = false;
        
        public override void Update()
        {
            foreach (var source in sources)
            {
                source.Update();
            }

            if (!_isDown && Value != 0.0f)
            {
                _isDown = true;
                Press();
            }

            if (_isDown && Value == 0.0f)
            {
                _isDown = false;
                Release();
            }
        }
    }
}