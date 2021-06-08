using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IRepositoryVR<T> : IDisposable where T : class
    {
        IQueryable<T> Query(string sql, params object[] parameters);

        T Search(params object[] keyValues);

        T Single(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true);

        T Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);

        T Update(T entity);
        T Delete(T entity);
        void Delete(IKey id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
