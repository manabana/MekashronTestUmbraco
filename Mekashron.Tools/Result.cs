namespace Mekashron.Tools
{
    public class Result
    {
        public Error? Error { get; }

        public Boolean IsSuccess => Error is null;

        internal Result(Error? error)
        {
            Error = error;
        }

        internal Result()
        {
            Error = null;
        }

        public static Result Success() => new();
        public static Result Failure(String message, String? key = null) => new(new Error(message, key));
        public static Result Failure(Error error) => new(error);
    }

    public class Result<T>: Result
    {
        public T? Value { get; }

        private Result(Error? error) : base(error)
        {
            Value = default;
        }

        private Result(T value) : base(null)
        {
            Value = value;
        }

        public Result FromTyped() => new Result(this.Error);

        public static Result<T> Success(T value) => new Result<T>(value);
        public static new Result<T> Failure(String message, String? key = null) => new(new Error(message, key));
        public new static Result<T> Failure(Error error) => new(error);
    }
}
