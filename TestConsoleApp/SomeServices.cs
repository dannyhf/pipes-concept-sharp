using System.Threading.Tasks;

namespace TestConsoleApp
{
    public interface IUserService
    {
        Task<Result<User>> UserById(int userId);
    }
    
    public interface IRequestService
    {
        Task<Result<int>> CreateRequestBy(User user);
    }
}