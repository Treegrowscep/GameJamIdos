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

    [Header("UI Buttons")]
    public string settingsButtonName = "SettingsButton"; // имя кнопки в Hierarchy

    void Start()
    {
        Debug.Log("MainMenu Start() called");

        if (settingsOverlay != null)
        {
            settingsOverlay.SetActive(false);
            Debug.Log("SettingsOverlay hidden at start");
        }

        if (videoOverlay != null)
        {
            videoOverlay.SetActive(false);
            Debug.Log("VideoOverlay hidden at start");
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            Debug.Log("VideoPlayer stopped at start");
        }
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings() called");

        // Проверяем, что клик был по SettingsButton
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        if (clicked != null && clicked.name == settingsButtonName)
        {
            Debug.Log("Confirmed click on SettingsButton");
            if (settingsOverlay != null)
                settingsOverlay.SetActive(true);

            if (videoOverlay != null)
                videoOverlay.SetActive(false);
        }
        else
        {
            Debug.Log("OpenSettings() ignored — not from SettingsButton");
        }
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings() called");

        if (settingsOverlay != null)
            settingsOverlay.SetActive(false);
    }

    public void PlayVideo()
    {
        Debug.Log("PlayVideo() called");

        if (settingsOverlay != null)
            settingsOverlay.SetActive(false);

        if (videoOverlay != null)
            videoOverlay.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            Debug.Log("VideoPlayer.Play() triggered");
        }
    }

    public void StopVideo()
    {
        Debug.Log("StopVideo() called");

        if (videoPlayer != null)
            videoPlayer.Stop();

        if (videoOverlay != null)
            videoOverlay.SetActive(false);
    }
}