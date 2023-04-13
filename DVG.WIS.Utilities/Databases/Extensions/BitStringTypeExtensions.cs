﻿// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NpgsqlTypes;
using System;

namespace DVG.WIS.Utilities.Databases.Extensions
{
    public static class BitStringTypeExtensions
    {
        public static PostgreSQLCopyHelper<TEntity> MapBit<TEntity>(this PostgreSQLCopyHelper<TEntity> helper, string columnName, Func<TEntity, bool> propertyGetter)
        {
            return helper.Map(columnName, propertyGetter, NpgsqlDbType.Bit);
        }

        public static PostgreSQLCopyHelper<TEntity> MapBit<TEntity>(this PostgreSQLCopyHelper<TEntity> helper, string columnName, Func<TEntity, bool?> propertyGetter)
        {
            return helper.Map(columnName, propertyGetter,  NpgsqlDbType.Bit);
        }
    }
}
