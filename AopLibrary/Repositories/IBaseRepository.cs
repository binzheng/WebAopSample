using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Repositories;


public interface IBaseRepository<T>
{
    T Add(T model);

    T Delete(T model);

    T Update(T model);
    T FindById(int id);

    IEnumerable<T> FindAll();
}
