namespace GameUtils.Tests
{
    /// <summary>
    /// Exposes VitalSystem dependencies and notifications for focused tests.
    /// </summary>
    public sealed class TestVitalSystem : VitalSystem
    {
        public int DestroyedHookCount { get; private set; }

        public void Configure(RuntimeVital vital, VoidEventAsset destroyedEvent)
        {
            // Inject runtime dependencies without relying on MonoBehaviour lifecycle setup.
            _vital = vital;
            _onDestroyed = destroyedEvent;
            _logEnabled = false;
        }

        protected override void OnDestroyed()
        {
            // Count the protected destruction hook for assertions.
            DestroyedHookCount++;
        }
    }
}
