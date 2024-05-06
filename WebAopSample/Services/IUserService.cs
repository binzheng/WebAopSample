using WebAopSample.Models;

namespace WebAopSample.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> FinalAll();

        UserModel Update(UserModel user);

        UserModel Insert(UserModel user);

        void Delete(String id);
    }
}
