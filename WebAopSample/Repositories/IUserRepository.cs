using AopLibrary.Repositories;
using System.Data.Common;
using WebAopSample.Models;

namespace WebAopSample.Repositories;

public interface IUserRepository : IBaseRepository<UserModel>
{
    IEnumerable<UserModel> FindUserByCond(string userName);


}
