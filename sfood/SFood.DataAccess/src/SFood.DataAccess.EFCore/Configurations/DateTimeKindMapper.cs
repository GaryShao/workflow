using System;

namespace SFood.DataAccess.EFCore.Configurations
{
    public class DateTimeKindMapper
    {
        public static DateTime Normalize(DateTime value)
            => DateTime.SpecifyKind(value, DateTimeKind.Utc);

        public static DateTime? NormalizeNullable(DateTime? value)
            => value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
    }
}