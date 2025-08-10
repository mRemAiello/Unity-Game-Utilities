#if UNITY_INCLUDE_TESTS
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace GameUtils.Tests
{
    public class LoggerFormatTests : ILoggable
    {
        public bool LogEnabled => true;

        [Test]
        public void LogFormatInterpolatesArguments()
        {
            LogAssert.Expect(LogType.Log, "[LoggerFormatTests] Value: 42");
            Logger.LogFormat("Value: {0}", this, null, 42);
        }

        [Test]
        public void LogWarningFormatInterpolatesArguments()
        {
            LogAssert.Expect(LogType.Warning, "[LoggerFormatTests] Value: 42");
            Logger.LogWarningFormat("Value: {0}", this, null, 42);
        }

        [Test]
        public void LogErrorFormatInterpolatesArguments()
        {
            LogAssert.Expect(LogType.Error, "[LoggerFormatTests] Value: 42");
            Logger.LogErrorFormat("Value: {0}", this, null, 42);
        }
    }
}
#endif
