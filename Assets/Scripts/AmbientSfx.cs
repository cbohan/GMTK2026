using System;
using UnityEngine;

public class AmbientSfx : MonoBehaviour
{
    [SerializeField] private AudioClip[] _ambientAudioClips;
    
    private AudioSource[] _ambientAudioSources;

    private void Start()
    {
        _ambientAudioSources = new AudioSource[_ambientAudioClips.Length];
        var index = 0;
        foreach (var clip in _ambientAudioClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = .33f;
            source.loop = true;
            source.playOnAwake = false;
            
            // Make the source 2d
            source.spatialBlend = 0f;
            
            source.Play();
            
            _ambientAudioSources[index++] = source;
        }
    }
}
