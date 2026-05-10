using UnityEngine;
using UnityEngine.Audio;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.AUDIO_NAME + "Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        [System.Serializable]
        public class AudioEntry
        {
            public string key;
            public AudioClip[] clips;
            [Range(0f, 1f)] public float volume = 1f;
            [Range(0f, 2f)] public float pitchMin = 0.95f;
            [Range(0f, 2f)] public float pitchMax = 1.05f;
        }

        [System.Serializable]
        public class MusicEntry
        {
            public string key;
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 1f;
            public float fadeDuration = 1f;
        }

        [System.Serializable]
        public class AmbientEntry
        {
            public string key;
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 1f;
        }

        [SerializeField] private AudioEntry[] _sfxEntries;
        [SerializeField] private MusicEntry[] _musicEntries;
        [SerializeField] private AmbientEntry[] _ambientEntries;
        [SerializeField] private AudioMixerGroup _sfxMixerGroup;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        [SerializeField] private AudioMixerGroup _ambientMixerGroup;

        public AudioEntry GetSFXEntry(string key)
        {
            foreach (var entry in _sfxEntries)
            {
                if (entry.key == key)
                    return entry;
            }
            return null;
        }

        public MusicEntry GetMusicEntry(string key)
        {
            foreach (var entry in _musicEntries)
            {
                if (entry.key == key)
                    return entry;
            }
            return null;
        }

        public AmbientEntry GetAmbientEntry(string key)
        {
            foreach (var entry in _ambientEntries)
            {
                if (entry.key == key)
                    return entry;
            }
            return null;
        }

        public AudioMixerGroup GetSFXMixerGroup() => _sfxMixerGroup;
        public AudioMixerGroup GetMusicMixerGroup() => _musicMixerGroup;
        public AudioMixerGroup GetAmbientMixerGroup() => _ambientMixerGroup;
    }
}
