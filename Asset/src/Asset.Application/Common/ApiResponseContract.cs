namespace Asset.Application.Common
{
    public class ApiResponseContract
    {
        public ApiResponseContract(ResultType _code, string _message) : this(_code, _message, string.Empty)
        {
        }

        public ApiResponseContract(ResultType _code, object _data) : this(_code, string.Empty, _data)
        {
        }

        public ApiResponseContract(ResultType _code, string _message, object _data)
        {
            Code = _code;
            Message = _message;
            Data = _data;
            Title = _code.ToString();
        }

        public ResultType Code { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }
    }
}
