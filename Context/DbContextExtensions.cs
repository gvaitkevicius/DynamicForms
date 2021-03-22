using System;
using System.Linq;

namespace DynamicForms.Context
{
    public static partial class DbContextExtensions
    {
        public static IQueryable<Object> Set(this JSgi _context, Type t)
        {
            return (IQueryable<Object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
        }

    }
}
