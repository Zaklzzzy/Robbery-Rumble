using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceCreator : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup fxGroup;

    public void CreateAudioSource(AudioResource _aRes)
    {
        var source = gameObject.AddComponent<AudioSource>();

        source.resource = _aRes;
        source.outputAudioMixerGroup = fxGroup;

        source.Play();

        Destroy(source, 10f);
    }
}
