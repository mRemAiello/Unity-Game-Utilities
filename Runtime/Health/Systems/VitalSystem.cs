using TriInspector;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Generic vital system for attributes like health, shields, barriers, runes, etc.
    /// Provides damage and restoration mechanics without automatic regeneration.
    /// </summary>
    [DeclareBoxGroup("Vital Events")]
    [DeclareBoxGroup("Settings")]
    public class VitalSystem : BaseVitalSystem
    {
        [SerializeField, Group("Vital Events")] protected GameEventAsset<float, object> _onDamaged;
        [SerializeField, Group("Vital Events")] protected GameEventAsset<float, object> _onRestored;
        [SerializeField, Group("Vital Events")] protected VoidEventAsset _onDestroyed;

        [SerializeField, Group("Settings")] protected bool _invulnerable = false;
        [SerializeField, Group("Settings"), ShowIf(nameof(_invulnerable))] protected float _invulnerabilityDuration = 0f;

        private bool _isInvulnerable = false;
        private float _invulnerabilityTimeRemaining = 0f;

        /// <summary>
        /// Returns true if the vital system is currently invulnerable.
        /// </summary>
        public bool IsInvulnerable => _isInvulnerable;

        protected virtual void Awake()
        {
            // Copy inspector configuration into runtime state without conflating it with remaining time.
            _isInvulnerable = _invulnerable;
            _invulnerabilityTimeRemaining = _isInvulnerable ? Mathf.Max(0f, _invulnerabilityDuration) : 0f;
        }

        protected virtual void Update()
        {
            // Advance only temporary invulnerability; a zero remainder represents a permanent state.
            UpdateInvulnerability(Time.deltaTime);
        }

        /// <summary>
        /// Advances a temporary invulnerability state by the supplied elapsed time.
        /// </summary>
        /// <param name="deltaTime">Elapsed time in seconds.</param>
        protected void UpdateInvulnerability(float deltaTime)
        {
            // Ignore permanent and inactive states so their state cannot end on a later frame.
            if (!_isInvulnerable || _invulnerabilityTimeRemaining <= 0f)
            {
                return;
            }

            _invulnerabilityTimeRemaining -= deltaTime;
            if (_invulnerabilityTimeRemaining <= 0f)
            {
                // End through the shared transition to guarantee a single notification.
                EndInvulnerability();
            }
        }

        /// <summary>
        /// Applies damage to the vital system.
        /// </summary>
        /// <param name="amount">Amount of damage to apply (positive value).</param>
        /// <param name="source">The source object that caused the damage (optional).</param>
        public virtual void TakeDamage(float amount, object source = null)
        {
            if (amount < 0)
            {
                this.LogWarning($"[{nameof(VitalSystem)}] TakeDamage called with negative amount ({amount}). Use Restore() instead.");
                return;
            }

            if (IsInvulnerable)
            {
                this.Log($"[{nameof(VitalSystem)}] TakeDamage blocked: {gameObject.name} is invulnerable.");
                return;
            }

            float oldValue = CurrentValue;
            SubtractValue(amount);
            float actualDamage = oldValue - CurrentValue;

            if (actualDamage > 0f)
            {
                this.Log($"[{nameof(VitalSystem)}] {gameObject.name} took {actualDamage} damage from {source?.ToString() ?? "unknown"}.");
                OnDamaged(actualDamage, source);
                _onDamaged?.Invoke(actualDamage, source);
            }
        }

        /// <summary>
        /// Restores the vital system by the specified amount.
        /// </summary>
        /// <param name="amount">Amount to restore (positive value).</param>
        /// <param name="source">The source object that caused the restoration (optional).</param>
        public virtual void Restore(float amount, object source = null)
        {
            if (amount < 0)
            {
                this.LogWarning($"[{nameof(VitalSystem)}] Restore called with negative amount ({amount}). Use TakeDamage() instead.");
                return;
            }

            float oldValue = CurrentValue;
            AddValue(amount);
            float actualRestored = CurrentValue - oldValue;

            if (actualRestored > 0f)
            {
                this.Log($"[{nameof(VitalSystem)}] {gameObject.name} restored {actualRestored} from {source?.ToString() ?? "unknown"}.");
                OnRestored(actualRestored, source);
                _onRestored?.Invoke(actualRestored, source);
            }
        }

        /// <summary>
        /// Immediately depletes the vital system to minimum value (e.g., death, destruction).
        /// </summary>
        public virtual void Destroy()
        {
            this.Log($"[{nameof(VitalSystem)}] {gameObject.name} destroyed.");
            SetToMin();
            OnDestroyed();
            _onDestroyed?.Invoke();
        }

        /// <summary>
        /// Sets the invulnerability state for the specified duration.
        /// </summary>
        /// <param name="enabled">Whether to enable or disable invulnerability.</param>
        /// <param name="duration">Duration of invulnerability in seconds (0 = permanent).</param>
        public virtual void SetInvulnerable(bool enabled, float duration = 0f)
        {
            if (enabled)
            {
                // A repeated enable updates the remaining duration without starting the state twice.
                _invulnerabilityTimeRemaining = Mathf.Max(0f, duration);
                if (!_isInvulnerable)
                {
                    _isInvulnerable = true;
                    this.Log($"[{nameof(VitalSystem)}] {gameObject.name} became invulnerable for {(duration > 0f ? duration + "s" : "permanent")}.");
                    OnInvulnerabilityStarted();
                }
            }
            else
            {
                // Use the same guarded transition used by automatic expiration.
                EndInvulnerability();
            }
        }

        /// <summary>
        /// Ends the active invulnerability state and emits its notification once.
        /// </summary>
        private void EndInvulnerability()
        {
            // Repeated disable or expiration calls are no-ops after the first transition.
            if (!_isInvulnerable)
            {
                return;
            }

            _isInvulnerable = false;
            _invulnerabilityTimeRemaining = 0f;
            this.Log($"[{nameof(VitalSystem)}] {gameObject.name} is no longer invulnerable.");
            OnInvulnerabilityEnded();
        }

        /// <summary>
        /// Called when damage is successfully applied.
        /// </summary>
        protected virtual void OnDamaged(float amount, object source) { }

        /// <summary>
        /// Called when restoration is successfully applied.
        /// </summary>
        protected virtual void OnRestored(float amount, object source) { }

        /// <summary>
        /// Called when the vital system is destroyed (reaches minimum value).
        /// </summary>
        protected virtual void OnDestroyed() { }

        /// <summary>
        /// Called when invulnerability starts.
        /// </summary>
        protected virtual void OnInvulnerabilityStarted() { }

        /// <summary>
        /// Called when invulnerability ends.
        /// </summary>
        protected virtual void OnInvulnerabilityEnded() { }

        protected override void OnMinReached()
        {
            base.OnMinReached();
            // Automatically trigger destruction when depleted.
            OnDestroyed();
            _onDestroyed?.Invoke();
        }
    }
}
