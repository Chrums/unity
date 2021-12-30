using System.IO;
using UnityEditor;

namespace Fizz6.Code
{
    public abstract class Node
    {
        public abstract string Generate();

        public void Generate(string path)
        {
            var value = Generate();
            if (File.Exists(path) && File.ReadAllText(path) == value) return;
            File.WriteAllText(path, value);
            AssetDatabase.Refresh();
        }
    }
}