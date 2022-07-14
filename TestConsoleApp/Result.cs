using System.Diagnostics;

namespace TestConsoleApp
{
    [DebuggerDisplay("[IsCorrect: {IsCorrect}]{Error}")]
	public class Result
	{
		public Error Error { get; set; }

		public bool IsCorrect => Error == null;
		public bool IsNotCorrect => !IsCorrect;
		
		public static implicit operator Result(MiErrorCodes error) => new Result(Error.FromCode(error));
        public static implicit operator Result(Error error) => new Result(error);

        public Result(Error error)
		{
			Error = error;
		}
	}

	[DebuggerDisplay("[IsCorrect: {IsCorrect}][Result: {Value}]{Error}")]
	public class Result<T> : Result
	{
		public Result(Error error) : base(error)
		{
		}
		
        public T Value { get; set; }

		public static implicit operator Result<T>(MiErrorCodes error) => new Result<T>(Error.FromCode(error));
        public static implicit operator Result<T>(Error error) => new Result<T>(error);
	}
	
	[DebuggerDisplay("[Code: {Code}][{Message}]")]
	public class Error
	{
		public MiErrorCodes Code { get; set; }

		public string Message { get; set; }

		public static implicit operator Error(MiErrorCodes error) => FromCode(error);
		public static implicit operator Error(Result result) => result.Error;

		public static Error FromCode(MiErrorCodes code, string message = null)
		{
			return new Error
			{
				Code = code,
				Message = message ?? "ERROR"
			};
		}
	}
	public enum MiErrorCodes
	{
		Error = 1
	}
}
