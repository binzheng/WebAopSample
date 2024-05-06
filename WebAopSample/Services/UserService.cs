using WebAopSample.Models;
using WebAopSample.Repositories;

namespace WebAopSample.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserModel> FinalAll()
    {
        throw new NotImplementedException();
    }

    public UserModel Insert(UserModel user)
    {
        return userRepository.Add(user);
    }

    public UserModel Update(UserModel user)
    {
        throw new NotImplementedException();
    }
}
