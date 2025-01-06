using UnityEngine;
using UnityEngine.Audio;

public class VolumeInitializer : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;

    private void Start()
    {
        // Load the saved volume
        float savedVolume = PlayerPrefs.GetFloat("AudioVolume", 1f);

        // Apply it to the mixer or global AudioListener
        Mixer.SetFloat("Volume", -80 + savedVolume * 80); // For Linear Mixer
        // Alternatively, if using AudioListener:
        // AudioListener.volume = savedVolume;
    }
}
