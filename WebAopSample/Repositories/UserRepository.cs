using AopLibrary.Repositories;
using Dapper;
using System.Data;
using WebAopSample.Models;

namespace WebAopSample.Repositories;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    public UserModel Add(UserModel user)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate("INSERT INTO MUser (Name, Email, Password, Phone) VALUES (@Name, @Email, @Password, @Phone) RETURNING *");
        return dbConnection.Query<UserModel>(template.RawSql, user).First<UserModel>();
    }

    public UserModel Delete(UserModel model)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserModel> FindAll()
    {
        throw new NotImplementedException();
    }

    public UserModel FindById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserModel> FindUserByCond(string userName)
    {
        throw new NotImplementedException();
    }

    public UserModel Update(UserModel model)
    {
        throw new NotImplementedException();
    }
}
