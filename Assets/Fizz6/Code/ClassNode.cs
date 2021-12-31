namespace Fizz6.Code
{
    public class ClassNode : BlockNode
    {
        public Accessibility Accessibility { get; set; }
        public string ClassName { get; set; }

        protected override string Statement => $"{Accessibility.ToAccessibilityString()} class {ClassName}";

        public ClassNode(string className = null)
        {
            ClassName = className;
        }
    }
}