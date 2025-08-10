#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameUtils
{
    public static class Logger
    {
        public static void Log(string message, ILoggable logger = null, Object context = null)
        {
            if (ILogCheck(logger, out var target, context))
            {
                Debug.Log(TryAppendClassName(target, message), target);
            }
        }

        public static void LogWarning(string message, ILoggable logger = null, Object context = null)
        {
            if (ILogCheck(logger, out var target, context))
            {
                Debug.LogWarning(TryAppendClassName(target, message), target);
            }
        }

        public static void LogError(string message, ILoggable logger = null, Object context = null)
        {
            if (ILogCheck(logger, out var target, context))
            {
                Debug.LogError(TryAppendClassName(target, message), target);
            }
        }

        public static void LogException(System.Exception exception, ILoggable logger = null, Object context = null)
        {
            if (ILogCheck(logger, out var target, context))
            {
                Debug.LogException(exception, target);
            }
        }

        public static void LogFormat(string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            if (ILogCheck(logger, out var target, context))
            {
                Debug.LogFormat(target, TryAppendClassName(target, format), args);
            }
        }

        public static void LogWarningFormat(string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            if (ILogCheck(logger, out var target, context))
            {
                Debug.LogWarningFormat(target, TryAppendClassName(target, format), args);
                TryPingObject(target);
            }
        }

        public static void LogErrorFormat(string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            if (ILogCheck(logger, out var target, context))
            {
                Debug.LogErrorFormat(target, TryAppendClassName(target, format), args);
                TryPingObject(target);
            }
        }

        public static void LogIf(bool condition, string message, ILoggable logger = null, Object context = null)
        {
            if (!condition)
                return;

            Log(message, logger, context);
        }

        public static void LogIfFormat(bool condition, string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            if (!condition)
                return;

            LogFormat(format, logger, context, args);
        }

        public static void LogWarningIf(bool condition, string message, ILoggable logger = null, Object context = null)
        {
            if (!condition)
                return;

            LogWarning(message, logger, context);
        }

        public static void LogWarningIfFormat(bool condition, string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            if (!condition)
                return;

            LogWarningFormat(format, logger, context, args);
        }

        public static void LogErrorIf(bool condition, string message, ILoggable logger = null, Object context = null)
        {
            if (!condition)
                return;

            LogError(message, logger, context);
        }

        public static void LogErrorIfFormat(bool condition, string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            if (!condition)
                return;

            LogErrorFormat(format, logger, context, args);
        }

        public static void LogExceptionIf(bool condition, System.Exception exception, ILoggable logger = null, Object context = null)
        {
            if (!condition)
                return;

            LogException(exception, logger, context);
        }

        public static void LogErrorAndThrow(string message, ILoggable logger = null, Object context = null)
        {
            LogError(message, logger, context);
            throw new System.Exception(message);
        }

        public static void LogErrorAndThrowFormat(string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            LogErrorFormat(format, logger, context, args);
            throw new System.Exception(string.Format(format, args));
        }

        public static void LogErrorAndThrowException(System.Exception exception, ILoggable logger = null, Object context = null)
        {
            LogException(exception, logger, context);
            throw exception;
        }

        public static void LogErrorAndThrowIf(bool condition, string message, ILoggable logger = null, Object context = null)
        {
            if (!condition)
                return;

            LogErrorAndThrow(message, logger, context);
        }

        public static void LogErrorAndThrowIfFormat(bool condition, string format, ILoggable logger = null, Object context = null, params object[] args)
        {
            if (!condition)
                return;

            LogErrorAndThrowFormat(format, logger, context, args);
        }

        public static void LogErrorAndThrowExceptionIf(bool condition, System.Exception exception, ILoggable logger = null, Object context = null)
        {
            if (!condition)
                return;

            LogErrorAndThrowException(exception, logger, context);
        }

        internal static bool ILogCheck(ILoggable logger, out Object target, Object context = null)
        {
            target = context != null ? context : (logger as Object);
            if (target is ILoggable logInstance) // Ensure target implements ILog
            {
                logger = logInstance;
                return logger.LogEnabled;
            }

            //
            Debug.LogWarning(TryAppendClassName(target, "Object is using Logger but does not implement ILog."), target);

            //
            return false;
        }

        internal static void TryPingObject(Object target)
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(target);
#endif
        }

        internal static string TryAppendClassName(Object target, string message)
        {
            if (target == null)
                return message;

            //
            return $"[{target.GetType().Name}] {message}";
        }
    }
}
