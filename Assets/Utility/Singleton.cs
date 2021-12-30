using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fizz6
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance => _instance ?? (_instance = new T());
    }
}