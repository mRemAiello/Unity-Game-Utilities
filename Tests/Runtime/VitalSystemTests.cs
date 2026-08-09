using NUnit.Framework;
using UnityEngine;

namespace GameUtils.Tests
{
    /// <summary>
    /// Verifies the runtime transitions of <see cref="VitalSystem"/> invulnerability.
    /// </summary>
    public class VitalSystemTests
    {
        private GameObject _gameObject;
        private TestVitalSystem _vitalSystem;

        /// <summary>
        /// Creates an isolated vital system before every test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Use a real component so the tests exercise the MonoBehaviour implementation.
            _gameObject = new GameObject(nameof(VitalSystemTests));
            _vitalSystem = _gameObject.AddComponent<TestVitalSystem>();
        }

        /// <summary>
        /// Destroys the test GameObject after every test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Destroy immediately because EditMode tests do not advance a player loop.
            Object.DestroyImmediate(_gameObject);
        }

        /// <summary>
        /// Confirms a zero-duration state remains active until explicitly disabled.
        /// </summary>
        [Test]
        public void PermanentInvulnerabilityDoesNotExpire()
        {
            // Advance far beyond any plausible temporary duration.
            _vitalSystem.SetInvulnerable(true, 0f);
            _vitalSystem.AdvanceInvulnerability(100f);
            _vitalSystem.TakeDamage(10f);

            Assert.That(_vitalSystem.IsInvulnerable, Is.True);
            Assert.That(_vitalSystem.EndedCount, Is.Zero);
            Assert.That(_vitalSystem.DamageApplicationCount, Is.Zero);

            // Damage reaches the vital implementation after explicit deactivation.
            _vitalSystem.SetInvulnerable(false);
            _vitalSystem.TakeDamage(10f);
            Assert.That(_vitalSystem.DamageApplicationCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Confirms a positive-duration state expires exactly once.
        /// </summary>
        [Test]
        public void TemporaryInvulnerabilityExpiresOnce()
        {
            // Cross the configured duration and then advance another frame.
            _vitalSystem.SetInvulnerable(true, 1f);
            _vitalSystem.AdvanceInvulnerability(0.5f);
            Assert.That(_vitalSystem.IsInvulnerable, Is.True);

            _vitalSystem.AdvanceInvulnerability(0.5f);
            _vitalSystem.AdvanceInvulnerability(1f);

            Assert.That(_vitalSystem.IsInvulnerable, Is.False);
            Assert.That(_vitalSystem.EndedCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Confirms manual disabling ends an active temporary state once.
        /// </summary>
        [Test]
        public void ManualDisableCancelsTemporaryInvulnerability()
        {
            // Disable before the timer can expire and ensure later updates remain inert.
            _vitalSystem.SetInvulnerable(true, 10f);
            _vitalSystem.SetInvulnerable(false);
            _vitalSystem.AdvanceInvulnerability(10f);

            Assert.That(_vitalSystem.IsInvulnerable, Is.False);
            Assert.That(_vitalSystem.EndedCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Confirms repeated calls do not duplicate lifecycle notifications.
        /// </summary>
        [Test]
        public void RepeatedCallsEmitEachTransitionOnce()
        {
            // Refresh the active state and disable it repeatedly.
            _vitalSystem.SetInvulnerable(true, 1f);
            _vitalSystem.SetInvulnerable(true, 2f);
            _vitalSystem.SetInvulnerable(false);
            _vitalSystem.SetInvulnerable(false);

            Assert.That(_vitalSystem.StartedCount, Is.EqualTo(1));
            Assert.That(_vitalSystem.EndedCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Test seam that exposes deterministic time advancement and hook counts.
        /// </summary>
        private sealed class TestVitalSystem : VitalSystem
        {
            public int StartedCount { get; private set; }
            public int EndedCount { get; private set; }
            public int DamageApplicationCount { get; private set; }

            /// <summary>
            /// Advances the protected invulnerability timer for an EditMode test.
            /// </summary>
            public void AdvanceInvulnerability(float deltaTime)
            {
                // Delegate to production timer logic rather than reproducing it in the test.
                UpdateInvulnerability(deltaTime);
            }

            /// <summary>
            /// Records attempts to apply damage after the invulnerability guard.
            /// </summary>
            public override void SubtractValue(float amount)
            {
                // Avoid requiring attribute assets while still observing the damage path.
                DamageApplicationCount++;
            }

            /// <summary>
            /// Counts invulnerability start transitions.
            /// </summary>
            protected override void OnInvulnerabilityStarted()
            {
                // Record the hook call for assertions.
                StartedCount++;
            }

            /// <summary>
            /// Counts invulnerability end transitions.
            /// </summary>
            protected override void OnInvulnerabilityEnded()
            {
                // Record the hook call for assertions.
                EndedCount++;
            }
        }
    }
}
