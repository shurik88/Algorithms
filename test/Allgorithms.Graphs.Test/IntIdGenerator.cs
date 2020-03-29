namespace Algorithms.Graphs.Test
{
    public class IntIdGenerator
    {
        private int _value;
        public IntIdGenerator(int initValue = 0)
        {
            _value = initValue;
        }

        public int Next { get { return ++_value; } }
    }
}
