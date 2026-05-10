using UnityEngine;

namespace GameUtils
{
    public static class LogExtensions
    {
        public static void Log(this ILoggable logger, string message, Object context = null)
        {
            Logger.Log(message, logger, context);
        }

        public static void LogWarning(this ILoggable logger, string message, Object context = null)
        {
            Logger.LogWarning(message, logger, context);
        }

        public static void LogError(this ILoggable logger, string message, Object context = null)
        {
            Logger.LogError(message, logger, context);
        }

        public static void LogException(this ILoggable logger, System.Exception exception, Object context = null)
        {
            Logger.LogException(exception, logger, context);
        }

        public static void LogFormat(
            this ILoggable logger,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogFormat(format, logger, context, args);
        }

        public static void LogWarningFormat(
            this ILoggable logger,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogWarningFormat(format, logger, context, args);
        }

        public static void LogErrorFormat(
            this ILoggable logger,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogErrorFormat(format, logger, context, args);
        }

        public static void LogIf(
            this ILoggable logger,
            bool condition,
            string message,
            Object context = null
        )
        {
            Logger.LogIf(condition, message, logger, context);
        }

        public static void LogIfFormat(
            this ILoggable logger,
            bool condition,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogIfFormat(condition, format, logger, context, args);
        }

        public static void LogWarningIf(
            this ILoggable logger,
            bool condition,
            string message,
            Object context = null
        )
        {
            Logger.LogWarningIf(condition, message, logger, context);
        }

        public static void LogWarningIfFormat(
            this ILoggable logger,
            bool condition,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogWarningIfFormat(condition, format, logger, context, args);
        }

        public static void LogErrorIf(
            this ILoggable logger,
            bool condition,
            string message,
            Object context = null
        )
        {
            Logger.LogErrorIf(condition, message, logger, context);
        }

        public static void LogErrorIfFormat(
            this ILoggable logger,
            bool condition,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogErrorIfFormat(condition, format, logger, context, args);
        }

        public static void LogExceptionIf(
            this ILoggable logger,
            bool condition,
            System.Exception exception,
            Object context = null
        )
        {
            Logger.LogExceptionIf(condition, exception, logger, context);
        }

        public static void LogErrorAndThrow(this ILoggable logger, string message, Object context = null)
        {
            Logger.LogErrorAndThrow(message, logger, context);
        }

        public static void LogErrorAndThrowFormat(
            this ILoggable logger,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogErrorAndThrowFormat(format, logger, context, args);
        }

        public static void LogErrorAndThrowException(
            this ILoggable logger,
            System.Exception exception,
            Object context = null
        )
        {
            Logger.LogErrorAndThrowException(exception, logger, context);
        }

        public static void LogErrorAndThrowIf(
            this ILoggable logger,
            bool condition,
            string message,
            Object context = null
        )
        {
            Logger.LogErrorAndThrowIf(condition, message, logger, context);
        }

        public static void LogErrorAndThrowIfFormat(
            this ILoggable logger,
            bool condition,
            string format,
            Object context = null,
            params object[] args
        )
        {
            Logger.LogErrorAndThrowIfFormat(condition, format, logger, context, args);
        }

        public static void LogErrorAndThrowExceptionIf(this ILoggable logger, bool condition, System.Exception exception, Object context = null)
        {
            Logger.LogErrorAndThrowExceptionIf(condition, exception, logger, context);
        }
    }
}
