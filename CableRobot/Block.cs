using System.Linq;

namespace CableRobot
{
    public class Block<T>
    {
        public Block(string name, T[] elements)
        {
            Name = name;
            Elements = elements;
        }

        public string Name { get; }
        public T[] Elements { get; }

        public override string ToString() => $"\n# {Name}\n\n{string.Join("\n", Elements.Select(_ => _.ToString()))}";
    }
}