using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Toggle fullscreenToggle;
    public Slider musicSlider;
    public Slider soundSlider;

    void Start()
    {
        // Obtener el estado actual de la pantalla completa y actualizar el toggle
        fullscreenToggle.isOn = Screen.fullScreen;

        // Obtener el volumen de la música y los efectos de sonido y actualizar los sliders
        float musicVolume;
        audioMixer.GetFloat("MusicVolume", out musicVolume);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);

        float soundVolume;
        audioMixer.GetFloat("SoundVolume", out soundVolume);
        soundSlider.value = Mathf.Pow(10, soundVolume / 20);
    }

    // Método para cambiar el volumen de la música
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    // Método para cambiar el volumen de los efectos de sonido
    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }

    // Método para alternar la pantalla completa
    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
