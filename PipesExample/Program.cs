using System;
using System.Threading.Tasks;
using PipesExample.Pipes;
using PipesExample.Services;
using PipesExample.Models;

namespace PipesExample;

class Program
{
    private static readonly IUserService _userService = new UserService();
    private static readonly IRequestService _requestService = new RequestService();
    private static readonly IRequestNotifier _notifier = new RequestNotifier();
        
    static async Task Main()
    {
        var userId = 1;
        User user = null;
        var result = await userId
            .Start(_userService.UserById)
            .Get(u => user = u)
            .Continue(_requestService.CreateRequestBy, u => u.Id)
            .Skip(_notifier.Notify);

        Console.WriteLine(result.Value);
        Console.WriteLine(user.Name);
    }
}