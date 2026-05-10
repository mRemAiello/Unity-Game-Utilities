using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace GameUtils
{
    /// <summary>
    /// Manages a pool of AudioSources for efficient SFX playback.
    /// </summary>
    public class AudioSourcePool
    {
        private Queue<AudioSource> _availableSources = new();
        private HashSet<AudioSource> _activeSources = new();
        private Transform _poolParent;
        private AudioMixerGroup _mixerGroup;
        private int _initialPoolSize;

        public AudioSourcePool(Transform parent, AudioMixerGroup mixerGroup, int initialPoolSize = 8)
        {
            _poolParent = parent;
            _mixerGroup = mixerGroup;
            _initialPoolSize = initialPoolSize;

            // Pre-allocate sources
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewSource();
            }
        }

        public AudioSource GetSource()
        {
            AudioSource source;

            if (_availableSources.Count > 0)
            {
                source = _availableSources.Dequeue();
            }
            else
            {
                source = CreateNewSource();
            }

            _activeSources.Add(source);
            source.gameObject.SetActive(true);
            return source;
        }

        public void ReturnSource(AudioSource source)
        {
            if (_activeSources.Remove(source))
            {
                source.Stop();
                source.clip = null;
                source.gameObject.SetActive(false);
                _availableSources.Enqueue(source);
            }
        }

        public void ReturnAllSources()
        {
            var activeSourcesList = new List<AudioSource>(_activeSources);
            foreach (var source in activeSourcesList)
            {
                ReturnSource(source);
            }
        }

        private AudioSource CreateNewSource()
        {
            GameObject sourceObj = new GameObject($"SFX Source {_availableSources.Count + _activeSources.Count}");
            sourceObj.transform.SetParent(_poolParent);
            sourceObj.SetActive(false);

            AudioSource source = sourceObj.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _mixerGroup;
            source.playOnAwake = false;

            _availableSources.Enqueue(source);
            return source;
        }

        public int GetActiveSourceCount() => _activeSources.Count;
        public int GetAvailableSourceCount() => _availableSources.Count;
    }
}
