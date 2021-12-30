namespace Fizz6.Code
{
    public class MethodNode : BlockNodeBase
    {
        public Accessibility Accessibility { get; set; }
        public string MethodReturnTypeName { get; set; }
        public string MethodName { get; set; }

        protected override string Statement => $"{Accessibility.ToAccessibilityString()} {MethodReturnTypeName} method {MethodName}";

        public MethodNode(string methodReturnTypeName = null, string methodName = null)
        {
            MethodReturnTypeName = methodReturnTypeName;
            MethodName = methodName;
        }
    }
}