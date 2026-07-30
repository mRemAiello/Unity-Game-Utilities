using NUnit.Framework;
using UnityEngine;

namespace GameUtils.Tests
{
    public class RegeneratingVitalSystemTests
    {
        private GameObject _gameObject;
        private TestRegeneratingVitalSystem _system;
        private AttributeData _attributeData;
        private VoidEventAsset _depletedEvent;
        private int _assetInvocationCount;

        /// <summary>
        /// Creates an initialized vital system and observes its depletion event asset.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject(nameof(RegeneratingVitalSystemTests));
            _system = _gameObject.AddComponent<TestRegeneratingVitalSystem>();
            _attributeData = ScriptableObject.CreateInstance<AttributeData>();
            _depletedEvent = ScriptableObject.CreateInstance<VoidEventAsset>();
            _depletedEvent.AddListener(_system, OnDepletedAssetInvoked);
            _system.Configure(new RuntimeVital(null, _attributeData, 10f), _depletedEvent);
        }

        /// <summary>
        /// Releases all Unity objects created by a test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_depletedEvent);
            Object.DestroyImmediate(_attributeData);
            Object.DestroyImmediate(_gameObject);
        }

        /// <summary>
        /// Verifies partial consumption does not report depletion.
        /// </summary>
        [Test]
        public void Consume_PartialAmount_DoesNotInvokeDepletedNotifications()
        {
            _system.Consume(4f);

            Assert.That(_system.CurrentValue, Is.EqualTo(6f));
            Assert.That(_system.DepletedHookInvocationCount, Is.Zero);
            Assert.That(_assetInvocationCount, Is.Zero);
        }

        /// <summary>
        /// Verifies exact consumption reports the transition to the minimum.
        /// </summary>
        [Test]
        public void Consume_AmountToMinimum_InvokesDepletedNotificationsOnce()
        {
            _system.Consume(10f);

            Assert.That(_system.CurrentValue, Is.EqualTo(_system.MinValue));
            Assert.That(_system.DepletedHookInvocationCount, Is.EqualTo(1));
            Assert.That(_assetInvocationCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Verifies excessive consumption is clamped and reports one depletion transition.
        /// </summary>
        [Test]
        public void Consume_AmountBeyondMinimum_InvokesDepletedNotificationsOnce()
        {
            _system.Consume(15f);

            Assert.That(_system.CurrentValue, Is.EqualTo(_system.MinValue));
            Assert.That(_system.DepletedHookInvocationCount, Is.EqualTo(1));
            Assert.That(_assetInvocationCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Verifies consumption attempts while already depleted do not repeat notifications.
        /// </summary>
        [Test]
        public void Consume_WhenAlreadyDepleted_DoesNotRepeatDepletedNotifications()
        {
            _system.Consume(10f);
            _system.Consume(1f);
            _system.Consume(5f);

            Assert.That(_system.CurrentValue, Is.EqualTo(_system.MinValue));
            Assert.That(_system.DepletedHookInvocationCount, Is.EqualTo(1));
            Assert.That(_assetInvocationCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Counts invocations emitted through the configured event asset.
        /// </summary>
        private void OnDepletedAssetInvoked()
        {
            _assetInvocationCount++;
        }

        private sealed class TestRegeneratingVitalSystem : RegeneratingVitalSystem
        {
            public int DepletedHookInvocationCount { get; private set; }

            /// <summary>
            /// Injects the test vital and event asset without requiring scene data.
            /// </summary>
            public void Configure(RuntimeVital vital, VoidEventAsset depletedEvent)
            {
                _vital = vital;
                _onDepleted = depletedEvent;
                _logEnabled = false;
            }

            /// <summary>
            /// Records calls to the protected depletion extension point.
            /// </summary>
            protected override void OnDepleted()
            {
                DepletedHookInvocationCount++;
            }
        }
    }
}
