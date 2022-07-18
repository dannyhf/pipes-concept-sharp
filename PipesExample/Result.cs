namespace PipesExample
{
	public class Result
	{
		public string Error { get; }

		public bool IsCorrect => Error == null;
		public bool IsNotCorrect => !IsCorrect;

		public Result(string error)
		{
			Error = error;
		}
	}

	public class Result<T> : Result
	{
		public Result(string error) : base(error)
		{ }

		public Result(T value) : base(null)
		{ Value = value; }
		
		public T Value { get; }
	}
}