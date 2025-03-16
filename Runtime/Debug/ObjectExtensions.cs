using UnityEngine;

namespace GameUtils
{
    public static class ObjectExtensions
    {
        public static void Log(this Object context, string message)
        {
            Logger.Log(message, null, context);
        }

        public static void LogWarning(this Object context, string message)
        {
            Logger.LogWarning(message, null, context);
        }

        public static void LogError(this Object context, string message)
        {
            Logger.LogError(message, null, context);
        }

        public static void LogException(this Object context, System.Exception exception)
        {
            Logger.LogException(exception, null, context);
        }

        public static void LogFormat(this Object context, string format, params object[] args)
        {
            Logger.LogFormat(format, null, context, args);
        }

        public static void LogWarningFormat(this Object context, string format, params object[] args)
        {
            Logger.LogWarningFormat(format, null, context, args);
        }

        public static void LogErrorFormat(this Object context, string format, params object[] args)
        {
            Logger.LogErrorFormat(format, null, context, args);
        }

        public static void LogIf(this Object context, bool condition, string message)
        {
            Logger.LogIf(condition, message, null, context);
        }

        public static void LogIfFormat(this Object context, bool condition, string format, params object[] args)
        {
            Logger.LogIfFormat(condition, format, null, context, args);
        }

        public static void LogWarningIf(this Object context, bool condition, string message)
        {
            Logger.LogWarningIf(condition, message, null, context);
        }

        public static void LogWarningIfFormat(this Object context, bool condition, string format, params object[] args)
        {
            Logger.LogWarningIfFormat(condition, format, null, context, args);
        }

        public static void LogErrorIf(this Object context, bool condition, string message)
        {
            Logger.LogErrorIf(condition, message, null, context);
        }

        public static void LogErrorIfFormat(this Object context, bool condition, string format, params object[] args)
        {
            Logger.LogErrorIfFormat(condition, format, null, context, args);
        }

        public static void LogExceptionIf(this Object context, bool condition, System.Exception exception)
        {
            Logger.LogExceptionIf(condition, exception, null);
        }

        public static void LogErrorAndThrow(this Object context, string message)
        {
            Logger.LogErrorAndThrow(message, null, context);
        }

        public static void LogErrorAndThrowFormat(this Object context, string format, params object[] args)
        {
            Logger.LogErrorAndThrowFormat(format, null, context, args);
        }

        public static void LogErrorAndThrowException(this Object context, System.Exception exception)
        {
            Logger.LogErrorAndThrowException(exception, null);
        }

        public static void LogErrorAndThrowIf(this Object context, bool condition, string message)
        {
            Logger.LogErrorAndThrowIf(condition, message, null, context);
        }

        public static void LogErrorAndThrowIfFormat(this Object context, bool condition, string format, params object[] args)
        {
            Logger.LogErrorAndThrowIfFormat(condition, format, null, context, args);
        }

        public static void LogErrorAndThrowExceptionIf(this Object context, bool condition, System.Exception exception)
        {
            Logger.LogErrorAndThrowExceptionIf(condition, exception, null, context);
        }
    }
}