namespace Fizz6.Code
{
    public class CodeBlockNode : BlockNode
    {
        public string Action { get; set; }
        protected override string Statement => Action;
    }
}