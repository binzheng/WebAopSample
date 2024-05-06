using Castle.Core.Logging;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Repositories;

/**
 * Todo: CRUDの実装
 * 
 */
public class BaseRepository<T>(IDbConnection dbConnection) : IBaseRepository<T> where T : class
{
    public T Add(T model)
    {
        dbConnection.Insert<T>(model);
        return model;
    }

    public T Delete(T model)
    {
        dbConnection.Delete<T>(model);
        return model;
    }

    public IEnumerable<T> FindAll()
    {
        return dbConnection.GetAll<T>();

    }

    public T FindById(int id)
    {
        return dbConnection.Get<T>(id);
    }

    public T Update(T model)
    {
        dbConnection.Update<T>(model);
        return model;
    }
}
