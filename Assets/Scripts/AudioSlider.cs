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

    private const string VolumePrefKey = "AudioVolume"; // Key for saving/loading volume

    private void Start()
    {
        if (slider != null)
        {
            // Load saved value from PlayerPrefs, default to 1.0f if not set
            float savedValue = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
            slider.value = savedValue;

            // Update audio settings based on the saved value
            ApplyVolume(savedValue);
        }
        else
        {
            Debug.LogError("Slider is not assigned in the Inspector!");
        }
    }

    public void OnChangeSlider(float value)
    {
        // Update UI text to show current slider value
        ValueText.SetText($"{value.ToString("N2")}");

        // Apply the volume settings
        ApplyVolume(value);

        // Save the value to PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, value);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float value)
    {
        switch (MixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                AudioSource.volume = value;
                break;
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", -80 + value * 80); // Linear mapping
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(value) * 20); // Logarithmic mapping
                break;
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
        if (slider != null && slider.value == 0)
        {
            AudioSource.volume = 0f;
        }
    }
}
