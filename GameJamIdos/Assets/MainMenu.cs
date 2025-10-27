using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject settingsOverlay;
    public GameObject videoOverlay;

    [Header("Video")]
    public VideoPlayer videoPlayer;

    [Header("Audio")]
    public AudioSource menuMusic;

    [Header("UI Buttons")]
    public string settingsButtonName = "SettingsButton";

    void Start()
    {
        if (settingsOverlay != null) settingsOverlay.SetActive(false);
        if (videoOverlay != null) videoOverlay.SetActive(false);
        if (videoPlayer != null) videoPlayer.Stop();
        if (menuMusic != null && !menuMusic.isPlaying) menuMusic.Play();
    }

    public void OpenSettings()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        if (clicked != null && clicked.name == settingsButtonName)
        {
            if (settingsOverlay != null) settingsOverlay.SetActive(true);
            if (videoOverlay != null) videoOverlay.SetActive(false);
        }
    }

    public void CloseSettings()
    {
        if (settingsOverlay != null) settingsOverlay.SetActive(false);
    }

    public void PlayVideo()
    {
        if (settingsOverlay != null) settingsOverlay.SetActive(false);
        if (videoOverlay != null) videoOverlay.SetActive(true);
        if (videoPlayer != null) videoPlayer.Play();
        if (menuMusic != null && menuMusic.isPlaying) menuMusic.Stop();
    }

    public void StopVideo()
    {
        if (videoPlayer != null) videoPlayer.Stop();
        if (videoOverlay != null) videoOverlay.SetActive(false);
        if (menuMusic != null && !menuMusic.isPlaying) menuMusic.Play();
    }
}