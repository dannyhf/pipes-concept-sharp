using System;
using System.Threading.Tasks;

namespace PipesExample
{
    public class PipelineBuilder
    {
    
    }

    public class LinkingWrapper<T>
    {
        public bool IsLinked { get; }
        public bool IsUnlinked => !IsLinked;
        public T Value { get; }
        
        public LinkingWrapper(T value)
        {
            Value = value;
            IsLinked = true;
        }
        
        public LinkingWrapper()
        {
            IsLinked = false;
        }
    }

    public static class Pipes
    {
        public static async Task<Result<TOut>> Start<TOut, TIn>(this TIn value, Func<TIn, Task<Result<TOut>>> continuer) => 
            await continuer.Invoke(value);
        
        public static async Task<Result<TOut>> Continue<TOut, TIn>(this Task<Result<TIn>> previousOperation, 
            Func<TIn, Task<Result<TOut>>> continuer)
        {
            var previousResult = await previousOperation;
            
            if (previousResult.IsNotCorrect)
                return new Result<TOut>(previousResult.Error);

            return await continuer.Invoke(previousResult.Value);
        }
    
        public static async Task<Result<TOut>> Continue<TIn, TMiddle, TOut>(this Task<Result<TIn>> previousOperation, 
            Func<TMiddle, Task<Result<TOut>>> continuer, Func<TIn, TMiddle> mapper)
        {
            var previousResult = await previousOperation;
            
            if (previousResult.IsNotCorrect)
                return new Result<TOut>(previousResult.Error);

            var mapped = mapper.Invoke(previousResult.Value);
            
            return await continuer.Invoke(mapped);
        }
        
        public static async Task<Result<TIn>> Skip<TIn>(this Task<Result<TIn>> previousOperation, 
            Func<TIn, Task> invokable)
        {
            var previousResult = await previousOperation;
            
            if (previousResult.IsNotCorrect)
                return new Result<TIn>(previousResult.Error);

            await invokable.Invoke(previousResult.Value);

            return previousResult;
        }
        
        public static async Task<Result<TIn>> Skip<TIn, TUnimportant>(this Task<Result<TIn>> previousOperation, 
            Func<TIn, Task<Result<TUnimportant>>> invokable)
        {
            var previousResult = await previousOperation;
            
            if (previousResult.IsNotCorrect)
                return new Result<TIn>(previousResult.Error);

            var invocationResult = await invokable.Invoke(previousResult.Value);
            if(invocationResult.IsNotCorrect)
                return new Result<TIn>(invocationResult.Error);

            return previousResult;
        }

        public static async Task<Result<TIn>> Get<TIn>(this Task<Result<TIn>> previousOperation,
            Action<TIn> setter)
        {
            var result = await previousOperation;
            if(result.IsCorrect)
                setter.Invoke(result.Value);

            return result;
        }
    }
}