using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Generico.Models
{
    public interface IRepositorio<TEntity>: IDisposable
    {
        TEntity Create(TEntity toCreate);
        TEntity Retrieve(Expression<Func<TEntity, bool>> criterio);
        bool Update(TEntity toUpdate);
        bool Delete(TEntity toDelete);
        List<TEntity> Filter(Expression<Func<TEntity, bool>> criterio);
        
    }
}
