using NUnit.Framework;
using UnityEngine;

namespace GameUtils.Tests
{
    /// <summary>
    /// Verifies that vital depletion emits destruction notifications once per transition.
    /// </summary>
    public class VitalSystemTests
    {
        private GameObject _gameObject;
        private TestVitalSystem _system;
        private AttributeData _attributeData;
        private VoidEventAsset _destroyedEvent;
        private int _assetNotificationCount;

        [SetUp]
        public void SetUp()
        {
            // Build an isolated vital and subscribe to both destruction notification paths.
            _gameObject = new GameObject(nameof(VitalSystemTests));
            _system = _gameObject.AddComponent<TestVitalSystem>();
            _attributeData = ScriptableObject.CreateInstance<AttributeData>();
            JsonUtility.FromJsonOverwrite("{\"_minValue\":0,\"_maxValue\":100,\"_isVital\":true}", _attributeData);
            _destroyedEvent = ScriptableObject.CreateInstance<VoidEventAsset>();
            _destroyedEvent.AddListener(_system, CountAssetNotification);
            _system.Configure(new RuntimeVital(null, _attributeData, 100f), _destroyedEvent);
        }

        [TearDown]
        public void TearDown()
        {
            // Release Unity objects created for each test.
            Object.DestroyImmediate(_destroyedEvent);
            Object.DestroyImmediate(_attributeData);
            Object.DestroyImmediate(_gameObject);
        }

        [Test]
        public void LethalDamage_NotifiesOnce()
        {
            // Lethal damage must notify through the minimum transition only.
            _system.TakeDamage(100f);

            AssertDestroyedOnce();
        }

        [Test]
        public void Destroy_FromFullValue_NotifiesOnce()
        {
            // Explicit destruction must delegate to the same minimum transition.
            _system.Destroy();

            AssertDestroyedOnce();
        }

        [Test]
        public void Destroy_WhenAlreadyDepleted_DoesNotNotifyAgain()
        {
            // A vital already depleted by damage must treat destruction as a no-op.
            _system.TakeDamage(100f);
            _system.Destroy();

            AssertDestroyedOnce();
        }

        [Test]
        public void RepeatedDestroyCalls_DoNotDuplicateNotifications()
        {
            // Repeated commands must remain idempotent after the first transition.
            _system.Destroy();
            _system.Destroy();
            _system.Destroy();

            AssertDestroyedOnce();
        }

        private void CountAssetNotification()
        {
            // Track ScriptableObject event emissions separately from the virtual hook.
            _assetNotificationCount++;
        }

        private void AssertDestroyedOnce()
        {
            // Both public notification mechanisms must agree on exactly one emission.
            Assert.That(_system.DestroyedHookCount, Is.EqualTo(1));
            Assert.That(_assetNotificationCount, Is.EqualTo(1));
            Assert.That(_system.IsAtMin, Is.True);
        }

    }
}
