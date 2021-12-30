namespace Fizz6.Code
{
    public class NamespaceNode : BlockNodeBase
    {
        public string NamespaceName { get; set; }

        protected override string Statement => $"namespace {NamespaceName}";

        public NamespaceNode(string namespaceName = null)
        {
            NamespaceName = namespaceName;
        }
    }
}