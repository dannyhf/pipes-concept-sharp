using System;
using System.Threading.Tasks;

namespace PipesExample
{
    #region Interfaces

    public interface IUserService
    {
        Task<Result<User>> UserById(int userId);
    }

    public interface IRequestNotifier
    {
        Task Notify(int requestId);
    }

    public interface IRequestService
    {
        Task<Result<int>> CreateRequestBy(int userId);
    }

    #endregion

    #region Implementation

    public class UserService : IUserService
    {
        public async Task<Result<User>> UserById(int userId)
        {
            await Task.Delay(100);
            return new Result<User>(new User { Id = userId, Name = "Bob" });
        }
    }

    public class RequestService : IRequestService
    {
        public async Task<Result<int>> CreateRequestBy(int userId)
        {
            await Task.Delay(150);
            return new Result<int>(new Random(userId).Next(1, 1000));
        }
    }

    public class RequestNotifier : IRequestNotifier
    {
        public async Task Notify(int requestId)
        {
            await Task.Delay(150);
            Console.WriteLine($"Request {requestId} was created!");
        }
    }
    #endregion
}