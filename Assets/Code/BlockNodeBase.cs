using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fizz6.Code
{
    public abstract class BlockNodeBase<TNode> : Node, IEnumerable where TNode : Node
    {
        private const string Indentation = "    ";
        private const string BlockOpen = "{";
        private const string BlockClose = "}";

        protected abstract string Statement { get; }

        private readonly List<TNode> _children = new List<TNode>();

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public override string Generate()
        {
            if (Statement != null)
            {
                _stringBuilder.AppendLine(Statement);
            }
            
            _stringBuilder.AppendLine(BlockOpen);

            foreach (var value in _children.Select(child => child.Generate()))
            {
                var replace = $"{Indentation}{value.Replace("\n", $"\n{Indentation}")}";
                _stringBuilder.AppendLine(replace);
            }

            _stringBuilder.AppendLine(BlockClose);

            return _stringBuilder.ToString().Trim('\n');
        }
    
        #region IEnumerable

        public void Add(TNode node) => _children.Add(node);
        public bool Remove(TNode node) => _children.Remove(node);
        
        public IEnumerator<Node> GetEnumerator() => _children.GetEnumerator();
    
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #endregion
    }
    
    public abstract class BlockNodeBase : BlockNodeBase<Node> {}
    
    public class BlockNode : BlockNodeBase
    {
        public string Action { get; set; }
        protected override string Statement => Action;
    }
}