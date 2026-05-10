using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameUtils
{
    /// <summary>
    /// Centralized audio manager for handling SFX, Music, and Ambient audio playback.
    /// Provides object pooling for SFX, fade in/out for music, and volume controls.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        private static AudioManager _instance;
        public static AudioManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        #endregion

        [SerializeField] private AudioConfig _audioConfig;
        [SerializeField] private int _sfxPoolSize = 16;

        private AudioSourcePool _sfxPool;
        private AudioSource _musicSource;
        private AudioSource _ambientSource;

        private Coroutine _musicFadeCoroutine;
        private Coroutine _ambientFadeCoroutine;

        private bool _isMuted = false;
        private bool _isPaused = false;

        private float _masterVolume = 1f;
        private float _sfxVolume = 1f;
        private float _musicVolume = 1f;
        private float _ambientVolume = 1f;

        private string _currentMusicKey;
        private string _currentAmbientKey;

        private void Initialize()
        {
            if (_audioConfig == null)
            {
                Debug.LogError("AudioConfig is not assigned to AudioManager!");
                return;
            }

            // Create SFX pool
            Transform poolParent = new GameObject("SFX Pool").transform;
            poolParent.SetParent(transform);
            //_sfxPool = new AudioSourcePool(poolParent, _audioConfig.GetSFXMixerGroup(), _sfxPoolSize);

            // Create Music source
            GameObject musicObj = new("Music Source");
            musicObj.transform.SetParent(transform);
            _musicSource = musicObj.AddComponent<AudioSource>();
            _musicSource.outputAudioMixerGroup = _audioConfig.GetMusicMixerGroup();
            _musicSource.playOnAwake = false;
            _musicSource.loop = false;
            _musicSource.spatialBlend = 0f;

            // Create Ambient source
            GameObject ambientObj = new("Ambient Source");
            ambientObj.transform.SetParent(transform);
            _ambientSource = ambientObj.AddComponent<AudioSource>();
            _ambientSource.outputAudioMixerGroup = _audioConfig.GetAmbientMixerGroup();
            _ambientSource.playOnAwake = false;
            _ambientSource.loop = true;
            _ambientSource.spatialBlend = 0f;
        }

        #region SFX
        /// <summary>
        /// Plays a 2D sound effect by key from the audio config.
        /// Picks a random clip from the entry's clip array and applies pitch variance.
        /// </summary>
        public void PlaySFX(string key)
        {
            var entry = _audioConfig.GetSFXEntry(key);
            if (entry == null || entry.clips.Length == 0)
            {
                Debug.LogWarning($"SFX entry '{key}' not found or has no clips!");
                return;
            }

            PlaySFXInternal(entry, null);
        }

        /// <summary>
        /// Plays a 3D sound effect by key at the specified world position.
        /// Picks a random clip from the entry's clip array and applies pitch variance.
        /// </summary>
        public void PlaySFX(string key, Vector3 worldPosition)
        {
            var entry = _audioConfig.GetSFXEntry(key);
            if (entry == null || entry.clips.Length == 0)
            {
                Debug.LogWarning($"SFX entry '{key}' not found or has no clips!");
                return;
            }

            PlaySFXInternal(entry, worldPosition);
        }

        private void PlaySFXInternal(AudioConfig.AudioEntry entry, Vector3? worldPosition)
        {
            AudioSource source = _sfxPool.GetSource();

            // Pick random clip
            AudioClip clip = entry.clips[Random.Range(0, entry.clips.Length)];
            source.clip = clip;

            // Apply pitch variance
            float pitch = Random.Range(entry.pitchMin, entry.pitchMax);
            source.pitch = pitch;

            // Set volume
            source.volume = entry.volume * _sfxVolume * _masterVolume;

            // Set spatial audio if position provided
            if (worldPosition.HasValue)
            {
                source.spatialBlend = 1f;
                source.transform.position = worldPosition.Value;
            }
            else
            {
                source.spatialBlend = 0f;
            }

            // Play and schedule return to pool
            source.Play();
            StartCoroutine(ReturnToPoolAfterPlay(source, clip.length));
        }

        /// <summary>
        /// Stops all active SFX and returns them to the pool.
        /// </summary>
        public void StopAllSFX()
        {
            _sfxPool.ReturnAllSources();
        }

        private IEnumerator ReturnToPoolAfterPlay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            _sfxPool.ReturnSource(source);
        }
        #endregion

        #region Music
        /// <summary>
        /// Plays a music track by key. Fades out current music first if one is playing.
        /// </summary>
        public void PlayMusic(string key, bool fadeIn = true)
        {
            var entry = _audioConfig.GetMusicEntry(key);
            if (entry == null)
            {
                Debug.LogWarning($"Music entry '{key}' not found!");
                return;
            }

            _currentMusicKey = key;

            // Stop current music
            if (_musicSource.isPlaying)
            {
                StopMusic(fadeOut: true);
            }

            StartCoroutine(PlayMusicCoroutine(entry, fadeIn));
        }

        private IEnumerator PlayMusicCoroutine(AudioConfig.MusicEntry entry, bool fadeIn)
        {
            _musicSource.clip = entry.clip;
            _musicSource.volume = 0f;
            _musicSource.Play();

            if (fadeIn)
            {
                yield return FadeAudio(_musicSource, entry.volume, entry.fadeDuration);
            }
            else
            {
                _musicSource.volume = entry.volume * _musicVolume * _masterVolume;
            }
        }

        /// <summary>
        /// Stops the current music with optional fade out.
        /// </summary>
        public void StopMusic(bool fadeOut = true)
        {
            if (!_musicSource.isPlaying)
                return;

            if (fadeOut)
            {
                var entry = _audioConfig.GetMusicEntry(_currentMusicKey);
                if (entry != null)
                {
                    StartCoroutine(StopMusicCoroutine(entry.fadeDuration));
                }
                else
                {
                    _musicSource.Stop();
                }
            }
            else
            {
                _musicSource.Stop();
            }

            _currentMusicKey = null;
        }

        private IEnumerator StopMusicCoroutine(float fadeDuration)
        {
            yield return FadeAudio(_musicSource, 0f, fadeDuration);
            _musicSource.Stop();
        }

        /// <summary>
        /// Pauses the currently playing music.
        /// </summary>
        public void PauseMusic()
        {
            if (_musicSource.isPlaying)
                _musicSource.Pause();
        }

        /// <summary>
        /// Resumes the paused music.
        /// </summary>
        public void ResumeMusic()
        {
            if (!_musicSource.isPlaying && _musicSource.clip != null)
                _musicSource.Play();
        }
        #endregion

        #region Ambient
        /// <summary>
        /// Plays ambient audio by key in a loop.
        /// </summary>
        public void PlayAmbient(string key)
        {
            var entry = _audioConfig.GetAmbientEntry(key);
            if (entry == null)
            {
                Debug.LogWarning($"Ambient entry '{key}' not found!");
                return;
            }

            _currentAmbientKey = key;

            _ambientSource.clip = entry.clip;
            _ambientSource.volume = entry.volume * _ambientVolume * _masterVolume;
            _ambientSource.Play();
        }

        /// <summary>
        /// Stops the currently playing ambient audio.
        /// </summary>
        public void StopAmbient()
        {
            if (_ambientSource.isPlaying)
                _ambientSource.Stop();

            _currentAmbientKey = null;
        }
        #endregion

        #region Volume Control
        /// <summary>
        /// Sets the master volume level (0-1).
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }

        /// <summary>
        /// Sets the SFX volume level (0-1).
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Sets the music volume level (0-1).
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            if (_musicSource.isPlaying)
            {
                _musicSource.volume = _musicVolume * _masterVolume;
            }
        }

        /// <summary>
        /// Sets the ambient volume level (0-1).
        /// </summary>
        public void SetAmbientVolume(float volume)
        {
            _ambientVolume = Mathf.Clamp01(volume);
            if (_ambientSource.isPlaying)
            {
                _ambientSource.volume = _ambientVolume * _masterVolume;
            }
        }

        private void UpdateAllVolumes()
        {
            if (_musicSource.isPlaying)
                _musicSource.volume = _musicVolume * _masterVolume;

            if (_ambientSource.isPlaying)
                _ambientSource.volume = _ambientVolume * _masterVolume;
        }
        #endregion

        #region Mute/Pause
        /// <summary>
        /// Mutes all audio.
        /// </summary>
        public void MuteAll()
        {
            _isMuted = true;
            _musicSource.mute = true;
            _ambientSource.mute = true;
        }

        /// <summary>
        /// Unmutes all audio.
        /// </summary>
        public void UnmuteAll()
        {
            _isMuted = false;
            _musicSource.mute = false;
            _ambientSource.mute = false;
        }

        /// <summary>
        /// Pauses all audio (useful for pause menu).
        /// </summary>
        public void PauseAll()
        {
            if (_isPaused)
                return;

            _isPaused = true;
            _musicSource.Pause();
            _ambientSource.Pause();
        }

        /// <summary>
        /// Resumes all paused audio.
        /// </summary>
        public void ResumeAll()
        {
            if (!_isPaused)
                return;

            _isPaused = false;

            if (_musicSource.clip != null)
                _musicSource.Play();

            if (_ambientSource.clip != null)
                _ambientSource.Play();
        }
        #endregion

        #region Fade Helper
        private IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
        {
            if (duration <= 0)
            {
                source.volume = targetVolume;
                yield break;
            }

            float startVolume = source.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                yield return null;
            }

            source.volume = targetVolume;
        }
        #endregion

        #region Getters
        public bool IsMuted => _isMuted;
        public bool IsPaused => _isPaused;
        public float MasterVolume => _masterVolume;
        public float SFXVolume => _sfxVolume;
        public float MusicVolume => _musicVolume;
        public float AmbientVolume => _ambientVolume;
        public string CurrentMusicKey => _currentMusicKey;
        public string CurrentAmbientKey => _currentAmbientKey;
        #endregion
    }
}
