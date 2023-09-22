using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    [Header("Audio")]

    [SerializeField] private AudioMixer _audioMixer;

    public void SetMusicVol(float volume) {
        ChangeVolume("Music", volume);
    }

    public void SetSfxVol(float volume) {
        ChangeVolume("SFX", volume);
    }

    private void ChangeVolume(string name, float volume) {
        _audioMixer.SetFloat(name, Mathf.Log10(volume) * 20);
    }

    public void ToggleWindowed() {
        Screen.fullScreen = !Screen.fullScreen;
    }

}
