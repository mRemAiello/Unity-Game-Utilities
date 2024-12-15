using UnityEngine;
using UnityEngine.Audio;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "GD/Audio/Play Audio")]
    public class PlayAudioScriptableObject : ScriptableObject
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private AudioMixerGroup _mixerGroup;

        public void Play()
        {
            GameObject gameObject = new($"Audio: {_audioClip.name}");

            //
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = _mixerGroup;
            source.clip = _audioClip;
            source.loop = false;
            source.spatialBlend = 0f;
            source.Play();

            //
            Destroy(gameObject, _audioClip.length);
        }
    }
}