namespace WebApplication.net.http
{
    public class ResultManager
    {
        private int _result;

        public ResultManager()
        {
            _result = 0;
        }

        public int GetResult()
        {
            return _result;
        }

        public void SetResult(int value)
        {
            _result = value;
        }

        public void AddToResult(int value)
        {
            _result += value;
        }

        public void SubtractFromResult(int value)
        {
            _result -= value;
        }
    }

}
