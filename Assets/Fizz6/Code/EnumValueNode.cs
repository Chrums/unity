namespace Fizz6.Code
{
    public class EnumValueNode : Node
    {
        public string EnumValueName { get; set; }

        public EnumValueNode(string enumValueName = null)
        {
            EnumValueName = enumValueName;
        }
        
        public override string Generate() => $"{EnumValueName},";
    }
}