namespace Mekashron.Tools
{
    public class Result<T>
    {
        public Error? Error { get; }
        public T? Value { get; }

        public Boolean IsSuccess => Error is null;

        private Result(Error? error)
        {
            Error = error;
            Value = default;
        }

        private Result(T value)
        {
            Value = value;
            Error = null;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(String message, String? key = null) => new Result<T>(new Error(message, key));
    }
}
