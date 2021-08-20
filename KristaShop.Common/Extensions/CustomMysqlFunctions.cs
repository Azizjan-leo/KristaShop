using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace KristaShop.Common.Extensions {
    public static class CustomMysqlFunctions {
        public static void InitializeMysqlCustomFunctions(this ModelBuilder builder) {
            _initializeRandomFunction(builder);
        }

        /// <summary>
        /// Invokes mysql rand() function
        /// </summary>
        public static int Random() => throw new InvalidOperationException($"{nameof(Random)} cannot be called client side");

        /// <summary>
        /// Initialization method for <see cref="Random"/> method to work
        /// </summary>
        private static void _initializeRandomFunction(ModelBuilder builder) {
            var randomOrderMethod = typeof(CustomMysqlFunctions).GetRuntimeMethod(nameof(Random), new Type[0]);

            builder
                .HasDbFunction(randomOrderMethod)
                .HasTranslation(args =>
                    SqlFunctionExpression.Create("rand", args, typeof(float), null)
                );
        }
    }
}
