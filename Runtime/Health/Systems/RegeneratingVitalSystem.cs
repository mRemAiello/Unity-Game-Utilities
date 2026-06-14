using System.Collections;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Generic regenerating vital system for attributes like mana, stamina, energy, spirit, etc.
    /// Supports automatic regeneration with configurable rate, delay, and consumption mechanics.
    /// </summary>
    [DeclareBoxGroup("Regeneration Events")]
    [DeclareBoxGroup("Regeneration Settings")]
    public class RegeneratingVitalSystem : BaseVitalSystem
    {
        [SerializeField, Group("Regeneration Events")] protected GameEventAsset<float, object> _onConsumed;
        [SerializeField, Group("Regeneration Events")] protected GameEventAsset<float, object> _onRestored;
        [SerializeField, Group("Regeneration Events")] protected VoidEventAsset _onRegenStarted;
        [SerializeField, Group("Regeneration Events")] protected VoidEventAsset _onRegenStopped;
        [SerializeField, Group("Regeneration Events")] protected VoidEventAsset _onDepleted;

        [SerializeField, Group("Regeneration Settings")] protected bool _autoRegenerate = true;
        [SerializeField, Group("Regeneration Settings"), ShowIf(nameof(_autoRegenerate))] protected float _regenRate = 5f;
        [SerializeField, Group("Regeneration Settings"), ShowIf(nameof(_autoRegenerate))] protected float _regenDelay = 1f;
        [SerializeField, Group("Regeneration Settings"), ShowIf(nameof(_autoRegenerate))] protected bool _regenWhileAtMax = false;

        private Coroutine _regenCoroutine;
        private float _timeSinceLastConsumption = 0f;
        private bool _isRegenerating = false;

        /// <summary>
        /// Returns true if the vital is currently regenerating.
        /// </summary>
        public bool IsRegenerating => _isRegenerating;

        protected virtual void Update()
        {
            if (_autoRegenerate && !_isRegenerating && !IsAtMax)
            {
                _timeSinceLastConsumption += Time.deltaTime;

                // Start regeneration after the delay period.
                if (_timeSinceLastConsumption >= _regenDelay)
                {
                    StartRegeneration();
                }
            }
        }

        /// <summary>
        /// Consumes the specified amount from the vital.
        /// </summary>
        /// <param name="amount">Amount to consume (positive value).</param>
        /// <param name="source">The source object that caused the consumption (optional).</param>
        public virtual void Consume(float amount, object source = null)
        {
            if (amount < 0)
            {
                this.LogWarning($"[{nameof(RegeneratingVitalSystem)}] Consume called with negative amount ({amount}). Use Restore() instead.");
                return;
            }

            float oldValue = CurrentValue;
            SubtractValue(amount);
            float actualConsumed = oldValue - CurrentValue;

            if (actualConsumed > 0f)
            {
                this.Log($"[{nameof(RegeneratingVitalSystem)}] {gameObject.name} consumed {actualConsumed} from {source?.ToString() ?? "unknown"}.");
                OnConsumed(actualConsumed, source);
                _onConsumed?.Invoke(actualConsumed, source);

                // Reset regeneration timer.
                _timeSinceLastConsumption = 0f;
                StopRegeneration();
            }
        }

        /// <summary>
        /// Attempts to consume the specified amount. Returns true if successful.
        /// </summary>
        /// <param name="amount">Amount to consume (positive value).</param>
        /// <param name="source">The source object that caused the consumption (optional).</param>
        /// <returns>True if there was enough vital to consume.</returns>
        public virtual bool TryConsume(float amount, object source = null)
        {
            if (CurrentValue >= amount)
            {
                Consume(amount, source);
                return true;
            }

            this.Log($"[{nameof(RegeneratingVitalSystem)}] {gameObject.name} failed to consume {amount}: insufficient vital ({CurrentValue}/{amount}).");
            return false;
        }

        /// <summary>
        /// Restores the vital by the specified amount.
        /// </summary>
        /// <param name="amount">Amount to restore (positive value).</param>
        /// <param name="source">The source object that caused the restoration (optional).</param>
        public virtual void Restore(float amount, object source = null)
        {
            if (amount < 0)
            {
                this.LogWarning($"[{nameof(RegeneratingVitalSystem)}] Restore called with negative amount ({amount}). Use Consume() instead.");
                return;
            }

            float oldValue = CurrentValue;
            AddValue(amount);
            float actualRestored = CurrentValue - oldValue;

            if (actualRestored > 0f)
            {
                this.Log($"[{nameof(RegeneratingVitalSystem)}] {gameObject.name} restored {actualRestored} from {source?.ToString() ?? "unknown"}.");
                OnRestored(actualRestored, source);
                _onRestored?.Invoke(actualRestored, source);
            }
        }

        /// <summary>
        /// Starts the regeneration coroutine.
        /// </summary>
        public virtual void StartRegeneration()
        {
            if (_isRegenerating || IsAtMax)
                return;

            _isRegenerating = true;
            _regenCoroutine = StartCoroutine(RegenerateCoroutine());
            this.Log($"[{nameof(RegeneratingVitalSystem)}] {gameObject.name} started regenerating.");
            OnRegenStarted();
            _onRegenStarted?.Invoke();
        }

        /// <summary>
        /// Stops the regeneration coroutine.
        /// </summary>
        public virtual void StopRegeneration()
        {
            if (!_isRegenerating)
                return;

            _isRegenerating = false;
            if (_regenCoroutine != null)
            {
                StopCoroutine(_regenCoroutine);
                _regenCoroutine = null;
            }

            this.Log($"[{nameof(RegeneratingVitalSystem)}] {gameObject.name} stopped regenerating.");
            OnRegenStopped();
            _onRegenStopped?.Invoke();
        }

        /// <summary>
        /// Coroutine that handles automatic regeneration over time.
        /// </summary>
        protected virtual IEnumerator RegenerateCoroutine()
        {
            while (_isRegenerating && (_regenWhileAtMax || !IsAtMax))
            {
                if (!IsAtMax)
                {
                    float regenAmount = _regenRate * Time.deltaTime;
                    AddValue(regenAmount);
                }

                yield return null;

                // Stop regenerating if we've reached max and shouldn't continue.
                if (IsAtMax && !_regenWhileAtMax)
                {
                    StopRegeneration();
                    yield break;
                }
            }

            _isRegenerating = false;
        }

        /// <summary>
        /// Called when consumption is successfully applied.
        /// </summary>
        protected virtual void OnConsumed(float amount, object source) { }

        /// <summary>
        /// Called when restoration is successfully applied.
        /// </summary>
        protected virtual void OnRestored(float amount, object source) { }

        /// <summary>
        /// Called when regeneration starts.
        /// </summary>
        protected virtual void OnRegenStarted() { }

        /// <summary>
        /// Called when regeneration stops.
        /// </summary>
        protected virtual void OnRegenStopped() { }

        protected override void OnMinReached()
        {
            base.OnMinReached();
            // Stop regeneration when depleted, will restart after delay.
            StopRegeneration();
            _timeSinceLastConsumption = 0f;
        }

        protected override void OnMaxReached()
        {
            base.OnMaxReached();
            // Stop regeneration when max is reached unless configured to continue.
            if (!_regenWhileAtMax)
            {
                StopRegeneration();
            }
        }

        protected virtual void OnDestroy()
        {
            // Clean up coroutine on destroy.
            if (_regenCoroutine != null)
            {
                StopCoroutine(_regenCoroutine);
                _regenCoroutine = null;
            }
        }
    }
}
