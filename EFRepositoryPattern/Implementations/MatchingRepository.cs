using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EFRepository.Queryable;

namespace EFRepository.Implementations
{
    public class MatchingRepository<T, TCriteria> : IRetrieveMatching<T, TCriteria>
        where T : class
        where TCriteria : class
    {
        private readonly IQueryable<T> _dbSet;
        private readonly Func<TCriteria, Expression<Func<T, bool>>> _expressionBuilder;

        public MatchingRepository(IQueryable<T> dbSet, Func<TCriteria, Expression<Func<T, bool>>> expressionBuilder)
        {
            _dbSet = dbSet;
            _expressionBuilder = expressionBuilder;
        }

        public IEnumerable<T> Retrieve(TCriteria criteria = null, params Order<T>[] orderBy)
        {
            return QueryHelpers<T>.BuildQuery(_dbSet, _expressionBuilder(criteria), orderBy);
        }
    }
}