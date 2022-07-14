
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Program
    {
        private static readonly IUserService _userService;
        private static readonly IRequestService _requestService;
        
        static async Task Main()
        {
            var userId = 1;
            var result = await _userService.UserById(userId)
                .Continue(_requestService.CreateRequestBy);
        }
    }
}