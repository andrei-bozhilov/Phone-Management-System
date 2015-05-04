namespace PhoneManagementSystem.Commons
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var propNames = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var propName in propNames)
            {
                // Use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = GetPropertyInfo(type, propName);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result =
                typeof(Queryable).GetMethods()
                    .Single(
                        method =>
                        method.Name == methodName && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }


        private static PropertyInfo GetPropertyInfo(Type type, string propName)
        {
            PropertyInfo result = null;
            var properies = type.GetProperties();

            foreach (var prop in properies)
            {
                if (prop.Name.ToLower() == propName.ToLower())
                {
                    return prop;
                }
            }

            return result;
        }
    }
}
