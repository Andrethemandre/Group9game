using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("--- Core Components ---")]
    public CameraController camController;
    public Conductor conductor;           
    public Follower follower;          
    public Fairy fairy;       

    [Header("--- dialogue ---")]
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    private bool hasGameStarted = false;

    void Start()
    {
        hasGameStarted = false;

        if (conductor != null) conductor.enabled = false;

        if (follower != null) follower.enabled = false;

        if (camController != null) camController.MoveToIntroView();

        ShowIntroDialog();
    }

    void ShowIntroDialog()
    {
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
            Debug.Log("DIALOGUE"); 
        }
        else
        {
            Debug.LogError("DialogPanel is not exist");
        }

        if (dialogText != null)
        {
            dialogText.text = "Click to Start!"; 
        }
    }

    public void OnDialogClicked()
    {
        if (hasGameStarted) return; 
        hasGameStarted = true;

        if (dialogPanel != null) dialogPanel.SetActive(false);

        if (camController != null)
        {
            camController.MoveToGameplayView(() =>
            {
                StartGameplayActual();
            });
        }
        else
        {
            StartGameplayActual();
        }
    }

    void StartGameplayActual()
    {
        Debug.Log("Game Start");
        if (conductor != null)
        {
            conductor.enabled = true;
            conductor.PlaySong();
        }

        if (follower != null) follower.enabled = true;
    }
}

