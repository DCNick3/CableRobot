using System.Linq;

namespace CableRobot
{
    /// <summary>
    /// Named section of CRC file
    /// Used to store metainformation about source of Commands, Points and Angles
    /// </summary>
    /// <typeparam name="T"></typeparam>
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