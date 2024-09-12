using System.Collections.Generic;

namespace WebApplication.net.http
{
    public class StackManager
    {
        private Stack<int> _stack;

        public StackManager()
        {
            _stack = new Stack<int>();
        }

        public void Push(int value)
        {
            _stack.Push(value);
        }

        public bool Pop(out int result)
        {
            if (_stack.Count > 0)
            {
                result = _stack.Pop();
                return true;
            }
            result = default;
            return false;
        }

        public int Peek()
        {
            return _stack.Count > 0 ? _stack.Peek() : default;
        }

        public int Count => _stack.Count;
    }
}
