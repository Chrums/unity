namespace Fizz6.Code
{
    public class NamespaceNode : BlockNode
    {
        public string NamespaceName { get; set; }

        protected override string Statement => $"namespace {NamespaceName}";

        public NamespaceNode(string namespaceName = null)
        {
            NamespaceName = namespaceName;
        }
    }
}