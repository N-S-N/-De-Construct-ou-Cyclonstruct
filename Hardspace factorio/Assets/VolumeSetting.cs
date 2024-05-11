using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    //[SerializeField] private Slider machineSlider;
    [SerializeField] private Slider sandEfectSlider;

    private void Start()
    {
        SetMusicVolumestart();
    }
    private void SetMusicVolumestart()
    {
        float volumeMusic = PlayerPrefs.GetFloat("musicVolum");
        float volumeEfecty = PlayerPrefs.GetFloat("EfectVolum");

        musicSlider.value = volumeMusic;
        sandEfectSlider.value = volumeEfecty;

        audioMixer.SetFloat("Music", math.log10(volumeMusic) * 20);
        audioMixer.SetFloat("SandEfect", math.log10(volumeEfecty) * 20);
    }
    public void SetMusicVolume()
    {
        float volumeMusic = musicSlider.value;
        float volumeEfecty = sandEfectSlider.value;

        PlayerPrefs.SetFloat("musicVolum", volumeMusic);
        PlayerPrefs.SetFloat("EfectVolum", volumeEfecty);

        audioMixer.SetFloat("Music", math.log10(volumeMusic)*20);
        audioMixer.SetFloat("SandEfect", math.log10(volumeEfecty) * 20);
    }

}
