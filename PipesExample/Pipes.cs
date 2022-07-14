using System;
using System.Threading.Tasks;

namespace PipesExample.Pipes;

public static class Pipes
{
    public static async Task<Result<TOut>> Start<TOut, TIn>(this TIn value, Func<TIn, Task<Result<TOut>>> continuer) => 
        await continuer.Invoke(value);
        
    public static async Task<Result<TOut>> Continue<TOut, TIn>(this Task<Result<TIn>> previousOperation, 
        Func<TIn, Task<Result<TOut>>> continuer)
    {
        var previousResult = await previousOperation;
            
        if (previousResult.IsNotCorrect)
            return previousResult.Error;

        return await continuer.Invoke(previousResult.Value);
    }
        
    public static async Task<Result<TIn>> Skip<TIn>(this Task<Result<TIn>> previousOperation, 
        Func<TIn, Task> continuer)
    {
        var previousResult = await previousOperation;
            
        if (previousResult.IsNotCorrect)
            return previousResult.Error;

        await continuer.Invoke(previousResult.Value);

        return previousResult;
    }
        
    public static async Task<Result<TOut>> Continue<TIn, TMiddle, TOut>(this Task<Result<TIn>> previousOperation, 
        Func<TMiddle, Task<Result<TOut>>> continuer, Func<TIn, TMiddle> mapper)
    {
        var previousResult = await previousOperation;
            
        if (previousResult.IsNotCorrect)
            return previousResult.Error;

        var mapped = mapper.Invoke(previousResult.Value);
            
        return await continuer.Invoke(mapped);
    }
}