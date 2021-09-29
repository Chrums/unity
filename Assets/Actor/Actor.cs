using UnityEngine;

namespace Fizz6
{
    public class Actor : MonoBehaviour
    {
        [SerializeReference, Implementation]
        private IBehaviour[] _behaviours;
        
        private IBehaviour _active;

        private void Update()
        {
            _active?.Update();
        }
    }
}