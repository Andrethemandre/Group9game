using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Loads the main game scene. 
    /// Ensure a scene named "MainGame" exists in your Build Settings.
    /// </summary>
    /// 
    [Header("--- UI  ---")]
    public GameObject optionsPanel;
    public Slider volumeSlider;

    void Start()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    /// <summary>
    /// Placeholder for Options logic.
    /// </summary>
    /// 
    public void OpenOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    /// <summary>
    /// Closes the application.
    /// Works in the built application and stops play mode in the Editor.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit Game requested.");

        Application.Quit();

        // This preprocessor directive ensures the editor stops playing when you click Quit
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }
}