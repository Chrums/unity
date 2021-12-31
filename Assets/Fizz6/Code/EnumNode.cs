namespace Fizz6.Code
{
    public class EnumNode : BlockNode<EnumValueNode>
    {
        public Accessibility Accessibility { get; set; }
        public string EnumName { get; set; }

        protected override string Statement => $"{Accessibility.ToAccessibilityString()} enum {EnumName}";

        public EnumNode(string enumName = null)
        {
            EnumName = enumName;
        }
    }
}