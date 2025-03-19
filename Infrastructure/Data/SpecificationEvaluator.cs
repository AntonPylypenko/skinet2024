using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        // обов'язково роби перевірку на нул, бо буде помилка 500 при запиті
        // (Internal Server Error. System.ArgumentNullException: Value cannot be null. )
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.IsDistinct)
        {
            query = query.Distinct();
        }

        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    } 

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        // обов'язково роби перевірку на нул, бо буде помилка 500 при запиті
        // (Internal Server Error. System.ArgumentNullException: Value cannot be null. )
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        var selectQuery = query as IQueryable<TResult>;

        if (spec.Select != null)
        {
            selectQuery = query.Select(spec.Select);
        }

        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        if (spec.IsPagingEnabled)
        {
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
        }

        return selectQuery ?? query.Cast<TResult>();
    }
}
