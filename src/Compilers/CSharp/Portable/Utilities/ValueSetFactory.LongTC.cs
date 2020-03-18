﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

#nullable enable

namespace Microsoft.CodeAnalysis.CSharp
{
    using static BinaryOperatorKind;

    internal static partial class ValueSetFactory
    {
        private struct LongTC : INumericTC<long>
        {
            long INumericTC<long>.MinValue => long.MinValue;

            long INumericTC<long>.MaxValue => long.MaxValue;

            (long leftMax, long rightMin) INumericTC<long>.Partition(long min, long max)
            {
                Debug.Assert(min < max);

                if (min == long.MinValue && max == long.MaxValue)
                    return (-1, 0);

                Debug.Assert((min < 0) == (max < 0));
                Debug.Assert(min != max);
                long half = (max - min) / 2;
                long leftMax = min + half;
                return (leftMax, leftMax + 1);
            }

            bool INumericTC<long>.Related(BinaryOperatorKind relation, long left, long right)
            {
                switch (relation)
                {
                    case Equal:
                        return left == right;
                    case GreaterThanOrEqual:
                        return left >= right;
                    case GreaterThan:
                        return left > right;
                    case LessThanOrEqual:
                        return left <= right;
                    case LessThan:
                        return left < right;
                    default:
                        throw new ArgumentException("relation");
                }
            }

            long INumericTC<long>.Next(long value)
            {
                Debug.Assert(value != long.MaxValue);
                return value + 1;
            }

            long INumericTC<long>.FromConstantValue(ConstantValue constantValue) => constantValue.Int64Value;

            string INumericTC<long>.ToString(long value) => value.ToString();
        }
    }
}