namespace Common.Extensions;

public static class Extensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingArgs pagingArgs)
    {
        return pagingArgs.UsePaging ? query.Skip(pagingArgs.Offset).Take(pagingArgs.Limit) : query;
    }
}