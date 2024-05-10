using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _name;

    private void Start()
    {
        float currentVolume;
        _audioMixer.GetFloat(_name,out currentVolume);
        gameObject.GetComponent<Slider>().value = currentVolume;
    }

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat(_name, volume);
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Slider>().onValueChanged.AddListener(SetVolume);
    }

    private void OnDisable()
    {
        gameObject.GetComponent<Slider>().onValueChanged.RemoveListener(SetVolume);
    }
}
