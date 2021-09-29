using System;
using UnityEngine;

namespace Fizz6
{
    [Serializable]
    public class TestBehaviour : IBehaviour
    {
        [SerializeField] private int test0;
        [SerializeField] private int test1;
        [SerializeField] private int[] test2;
        
        public void Start()
        {}

        public void Stop()
        {}

        public void Update()
        {}
    }
}