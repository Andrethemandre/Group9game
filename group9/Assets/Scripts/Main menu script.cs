using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Loads the main game scene. 
    /// Ensure a scene named "MainGame" exists in your Build Settings.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    /// <summary>
    /// Placeholder for Options logic.
    /// </summary>
    public void OpenOptions()
    {
        Debug.Log("Options Button Clicked - Logic to be implemented later.");
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
}