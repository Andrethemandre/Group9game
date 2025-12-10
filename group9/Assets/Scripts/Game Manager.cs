using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("--- Core Components ---")]
    public CameraController camController;
    public Conductor conductor;
    public Follower follower;

    [Header("--- UI Components ---")]
    public GameObject introDialogPanel;      
    public TextMeshProUGUI introText;   
    public GameObject musicFeedbackPanel;   

    [Header("--- Narratives ---")]
    [TextArea(2, 5)] 
    public string[] dialogueLines;

    [Header("--- End UI ---")]
    public GameObject endPanel;       
    public TextMeshProUGUI scoreText;    

    private int currentLineIndex = 0;
    private bool hasGameStarted = false;

    void Start()
    {
        hasGameStarted = false;
        currentLineIndex = 0;

        if (conductor != null) conductor.enabled = false;
        if (follower != null) follower.enabled = false;
        if (camController != null) camController.MoveToIntroView();

        if (musicFeedbackPanel != null) musicFeedbackPanel.SetActive(false);
        if (introDialogPanel != null) introDialogPanel.SetActive(true);

        ShowNextLine();
    }

    void Update()
    {
        if (!hasGameStarted && introDialogPanel != null && introDialogPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
            {
                OnIntroClicked();
            }
        }
    }

    public void OnIntroClicked()
    {
        if (hasGameStarted) return;

        if (currentLineIndex < dialogueLines.Length)
        {
            ShowNextLine();
        }
        else
        {
            StartGameTransition();
        }
    }

    void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            introText.text = dialogueLines[currentLineIndex];
            currentLineIndex++;
        }
    }

    void StartGameTransition()
    {
        hasGameStarted = true;

        if (introDialogPanel != null) introDialogPanel.SetActive(false);

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
        Debug.Log("Dialog End");
        if (conductor != null)
        {
            conductor.enabled = true;
            conductor.PlaySong();
        }
        if (follower != null) follower.enabled = true;
    }

    public void OnGameFinished(float accuracy)
    {
        Debug.Log(accuracy);

        if (conductor != null) conductor.enabled = false;

        if (endPanel != null) endPanel.SetActive(true);

        if (scoreText != null)
            scoreText.text = "Accuracy: " + (accuracy * 100).ToString("F0") + "%";

    }


    public void OnRetryClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnNextClicked()
    {
        Debug.Log("Quit");

        Application.Quit();
    }
}

