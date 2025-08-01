﻿using System;

namespace GameUtils
{
    internal static class ThrowErrors
    {
        public static ArgumentException SegmentOutOfRange()
        {
            return new ArgumentException("Segment is out of range.");
        }

        public static ArgumentException DestinationTooShort(string paramName)
        {
            return new ArgumentException("Destination too short.", paramName);
        }

        public static NotSupportedException ReadOnlyBitList()
        {
            return new NotSupportedException("BitList is read only.");
        }

        public static InvalidOperationException NoElements()
        {
            return new InvalidOperationException("Collection is empty.");
        }

        public static InvalidOperationException CollectionChanged()
        {
            return new InvalidOperationException("Collection has been changed.");
        }

        public static ArgumentOutOfRangeException NegativeParameter(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName, "The value cannot be negative.");
        }

        public static ArgumentOutOfRangeException ZeroParameter(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName, "Argument must be greater than zero.");
        }

        public static ArgumentNullException NullParameter(string paramName)
        {
            return new ArgumentNullException(paramName, "Value cannot be null.");
        }

        public static ArgumentException InvalidCurvePoints(string paramName, int requiredCount)
        {
            return new ArgumentException(paramName, $"Not enough points. There should be {requiredCount} at least.");
        }

        public static ArgumentOutOfRangeException OutOfRange(string paramName, string minName, string maxName)
        {
            return new ArgumentOutOfRangeException(paramName, $"The value cannot be out of range between {minName} and {maxName}.");
        }

        public static ArgumentException DifferentArrayLengths()
        {
            return new ArgumentException("Array lengths are not equal.");
        }

        public static IndexOutOfRangeException IndexOutOfRange()
        {
            return new IndexOutOfRangeException("The index is out of range.");
        }

        public static ArgumentOutOfRangeException MinMax(string minName, string maxName)
        {
            return new ArgumentOutOfRangeException(minName, $"{minName} cannot be greater than {maxName}.");
        }

        public static InvalidOperationException RangeDoesNotContain(string valuesName)
        {
            return new InvalidOperationException($"The range does not contain {valuesName} values.");
        }

        public static FormatException IncorrectInputString()
        {
            return new FormatException("Input string was not in a correct format.");
        }

        public static ArgumentOutOfRangeException RadixOutOfRange(string paramName, int symbolsLength)
        {
            return new ArgumentOutOfRangeException(paramName, $"The radix must be >= 2 and <= {symbolsLength}");
        }

        public static InvalidOperationException EmptyTracker()
        {
            return new InvalidOperationException("Tracker does not yet contain nodes.");
        }

        public static InvalidOperationException ContainsModifier()
        {
            return new InvalidOperationException("Modifier already added.");
        }

        public static InvalidOperationException DeadTask()
        {
            return new InvalidOperationException("Task isn't alive.");
        }
    }
}