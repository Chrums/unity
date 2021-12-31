using System.Collections.Generic;
using Fizz6.Utility;

namespace Fizz6.Input
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private readonly Dictionary<InputType, Source> _sources = new Dictionary<InputType, Source>();
        public IReadOnlyDictionary<InputType, Source> Sources => _sources;

        private void Awake()
        {
            foreach (var item in InputConfig.Instance.Items)
            {
                _sources[item.InputType] = item.Source;
            }
        }

        private void Update()
        {
            foreach (var source in _sources.Values)
            {
                source.Update();
            }
        }
    }
}