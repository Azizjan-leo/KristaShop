using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models.Structs;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KristaShop.Common.Implementation.DataAccess {
    public static class ValueConverters {
        public static ValueConverter SizeConverter = new ValueConverter<SizeValue, string>(
            obj => obj.Value,
            column => new SizeValue(column));
        
        public static ValueComparer SizeComparer = new ValueComparer<SizeValue>(
            (c1, c2) => c1.Value.Equals(c2.Value),
            size => size.Value.GetHashCode(), size => new SizeValue(size.Value, size.Line));

        public static ValueConverter MoneyDocumentTypeConverter = new ValueConverter<MoneyDocumentType, string>(
            obj => obj.GetDisplayName(),
            column => (MoneyDocumentType) column.GetSameHashCode());

        public static ValueConverter StringToDateConverter = new ValueConverter<DateTime, string>(
            obj => obj.ToSystemString(),
            column => DateTimeExtension.TryParseSystemString(column));
    }
}