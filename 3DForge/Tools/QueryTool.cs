using Backend3DForge.Requests;

namespace Backend3DForge.Tools
{
    public static class QueryTool
    {
        public static IQueryable<T> ToPaged<T>(this IQueryable<T> query, PageRequest request)
        {
            return query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
        }

        public static IQueryable<T> ToPaged<T>(this IQueryable<T> query, PageRequest request, out int totalItemsCount)
        {
            totalItemsCount = query.Count();
            return query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
        }
    }
}
