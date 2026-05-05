using TriInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameUtils
{
    [DeclareBoxGroup("Audio")]
    [DeclareBoxGroup("Debug")]
    public class AudioMixerVolumeSlider : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("Audio")] private AudioMixer _audioMixer;
        [SerializeField, Group("Audio")] private Slider _volumeSlider;
        [SerializeField, Group("Audio")] private string _parameterName = "MasterVol";
        [SerializeField, Group("Audio")] private string _saveContext = "AudioSettings";
        [SerializeField, Group("Debug")] private bool _logEnabled = true;

        //
        public bool LogEnabled => _logEnabled;

        //
        private void Start()
        {
            // Load
            float savedValue = GameSaveManager.Instance.Load<float>(_saveContext, _parameterName, 0);

            //
            this.Log($"Loading volume {_parameterName} from {savedValue}");

            //
            float dB = ConvertToDB(savedValue);
            _audioMixer.SetFloat(_parameterName, dB);
            _volumeSlider.value = savedValue;

            //  Listen for changes
            _volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        private float ConvertToDB(float linearValue)
        {
            // Avoid log(0)
            linearValue = Mathf.Clamp(linearValue, 0.0001f, 1f);

            // Convert to decibels
            return Mathf.Log10(linearValue) * 20f;
        }

        /// <summary>
        /// Sets the mixer volume by converting a linear value (0.0001-1) to decibels (-80dB to 0dB).
        /// </summary>
        /// <param name="linearValue">Slider value, ideally between 0.0001 and 1.</param>
        public void SetVolume(float linearValue)
        {
            // Convert the linear value to decibels
            float dB = ConvertToDB(linearValue);

            // Apply to the mixer parameter
            _audioMixer.SetFloat(_parameterName, dB);

            // Save
            GameSaveManager.Instance.Save(_saveContext, _parameterName, linearValue);
        }
    }
}