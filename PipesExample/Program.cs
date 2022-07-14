using System;
using System.Threading.Tasks;
using PipesExample.Pipes;
using PipesExample.Services;

namespace PipesExample;

class Program
{
    private static readonly IUserService _userService = new UserService();
    private static readonly IRequestService _requestService = new RequestService();
    private static readonly IRequestNotifier _notifier = new RequestNotifier();
        
    static async Task Main()
    {
        var userId = 1;
        var result = await userId
            .Start(_userService.UserById)
            .Continue(_requestService.CreateRequestBy, u => u.Id)
            .Skip(_notifier.Notify);

        Console.WriteLine(result.Value);
    }
}