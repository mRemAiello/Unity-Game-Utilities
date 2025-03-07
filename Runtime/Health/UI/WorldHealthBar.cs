using UnityEngine;
using UnityEngine.UI;

namespace GameUtils
{
    public class WorldHealthBar : MonoBehaviour
    {
       /*[Header("Components")]
        [SerializeField] private GameObject _uiRoot;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _fillHealthSlider;

        [Header("Options")]
        [SerializeField] private bool _lookAtCamera = true;

        [Space]
        [SerializeField] private float _healthFillSpeed;
        [SerializeField] private float _effectHealthFillSpeed;

        [Header("Events")]
        [SerializeField] private DamageInfoEventAsset _onDamage;

        //
        void Start()
        {
            //
            _healthSlider.minValue = 0;
            _fillHealthSlider.minValue = 0;
            _healthSlider.maxValue = 1;
            _fillHealthSlider.maxValue = 1;
            _healthSlider.value = 1;
            _fillHealthSlider.value = 1;
        }

        void Update()
        {
            if (_lookAtCamera)
            {
                _uiRoot.transform.rotation = Camera.main.transform.rotation;
            }

            //
            float target = Mathf.Lerp(_healthSlider.value, _currentHP, _healthFillSpeed * Time.deltaTime);
            float targetEffect = Mathf.Lerp(_fillHealthSlider.value, _currentHP, _effectHealthFillSpeed * Time.deltaTime);

            //
            _healthSlider.value = target;
            _fillHealthSlider.value = targetEffect;
        }
        /*[field: Header("Components")]
        [field: SerializeField] public Health TargetHealth { get; protected set; }
        [field: SerializeField] public Image FillBar { get; protected set; }
        [field: SerializeField] public GameObject UIRoot { get; protected set; }

        [field: Header("Options")]
        [field: SerializeField] public bool LookAtCamera { get; protected set; } = true;

        [field: Header("Animation")]
        [field: SerializeField] public bool AnimatedLerp { get; protected set; } = true;
        [field: SerializeField] public AnimationCurve Lerp { get; protected set; } = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.3f, 1f) });

        private Coroutine _currentLerp;



        protected virtual void OnValidate()
        {
            if(TargetHealth == null) TargetHealth = GetComponentInParent<Health>();
        }

        protected virtual void OnEnable()
        {
            if (TargetHealth == null) return;

            TargetHealth.OnDamage.AddListener(Damaged);
            TargetHealth.OnDeath.AddListener(Killed);
            FillBar.fillAmount = TargetHealth.Percentage;
        }

        protected virtual void OnDisable()
        {
            if (TargetHealth == null) return;

            TargetHealth.OnDamage.RemoveListener(Damaged);
            TargetHealth.OnDeath.RemoveListener(Killed);
        }

        protected virtual void Damaged(DamageInfo damageInfo)
        {
            if(_currentLerp != null) StopCoroutine(_currentLerp);
            _currentLerp = StartCoroutine(DamageLerpRoutine(FillBar.fillAmount, TargetHealth.Percentage));
        }

        private IEnumerator DamageLerpRoutine(float from, float to)
        {
            float duration = Lerp.keys[Lerp.length - 1].time;
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;
                float lerp = Lerp.Evaluate(progress);
                FillBar.fillAmount = Mathf.Lerp(from, to, lerp);

                yield return null;
            }
        }

        protected virtual void Killed(DamageInfo damageInfo)
        {
            UIRoot.SetActive(false);
        }

*/
    }
}