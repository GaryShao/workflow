using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SFood.DataAccess.EFCore.Configurations
{
    public class DateTimeKindEntityMaterializerSource : EntityMaterializerSource
    {
        private static readonly MethodInfo NormalizeMethod =
            typeof(DateTimeKindMapper).GetTypeInfo().GetMethod(nameof(DateTimeKindMapper.Normalize));

        private static readonly MethodInfo NormalizeNullableMethod =
            typeof(DateTimeKindMapper).GetTypeInfo().GetMethod(nameof(DateTimeKindMapper.NormalizeNullable));

        public override Expression CreateReadValueExpression(Expression valueBuffer, Type type, int index,
            IPropertyBase property)
        {
            if (type == typeof(DateTime))
            {
                return Expression.Call(NormalizeMethod,
                    base.CreateReadValueExpression(valueBuffer, type, index, property));
            }

            if (type == typeof(DateTime?))
            {
                return Expression.Call(NormalizeNullableMethod,
                    base.CreateReadValueExpression(valueBuffer, type, index, property));
            }

            return base.CreateReadValueExpression(valueBuffer, type, index, property);
        }
    }
}