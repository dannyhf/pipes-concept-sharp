using System;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    public static class Piper
    {
        public static async Task<Result<TOut>> Continue<TOut, TIn>(this Task<Result<TIn>> previousOperation, 
            Func<TIn, Task<Result<TOut>>> continuer)
        {
            var previousResult = await previousOperation;
            
            if (previousResult.IsNotCorrect)
                return previousResult.Error;

            return await continuer.Invoke(previousResult.Value);
        }
    } 
}