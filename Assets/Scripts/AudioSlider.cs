using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private TextMeshProUGUI ValueText;
    [SerializeField] private AudioMixMode MixMode;
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (slider != null)
        {
            slider.value = 1f; // Set the slider value to 1
        }
        else
        {
            Debug.LogError("Slider is not assigned in the Inspector!");
        }
    }

    public void OnChangeSlider(float value)
    {
        ValueText.SetText($"{value.ToString("N2")}");

        switch(MixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                AudioSource.volume = value; break;
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + value * 80)); break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(value) * 20); break;

        }
    }
    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }

    private void Update()
    {
        if (slider.value == 0)
        {
            AudioSource.volume = 0f;
        }
        else
        {
            AudioSource.volume = 0.75f;
        }
    }
}
