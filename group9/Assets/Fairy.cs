using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fairy : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] idleFrames;
    public Sprite[] talkingFrames;

    [Header("--- Dialogue Components ---")]
    public GameObject dialogPanel;       
    public TextMeshProUGUI dialogText;   

    [Header("--- Settings ---")]
    public float frameRate = 0.1f;
    public float displayDuration = 2f; 

    public string[] hitLines = { "Great!", "Perfect!", "Nice!", "You ate!", "Awesome!" };
    public string[] missLines = { "Oops...", "Come on!", "Cheer up!", "No..." };

    private float animTimer = 0f;
    private int currentFrameIndex = 0;

    private Image uiImage;
    private RectTransform rectTransform;
    private Vector2 originalPos;
    private float resetTimer = 0f;
    private bool isTalking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        originalPos = rectTransform.anchoredPosition;

        if (dialogPanel != null) dialogPanel.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation();

        float hover = Mathf.Sin(Time.time * 2.5f) * 8f;
        rectTransform.anchoredPosition = originalPos + new Vector2(0, hover);

        if (isTalking)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
            {
                ReturnToIdle();
            }
        }
    }

    void HandleAnimation()
    {
        animTimer += Time.deltaTime;

        Sprite[] currentClip = isTalking ? talkingFrames : idleFrames;

        if (currentClip == null || currentClip.Length == 0) return;

        if (animTimer >= frameRate)
        {
            animTimer = 0f;
            currentFrameIndex++;

            if (currentFrameIndex >= currentClip.Length)
            {
                currentFrameIndex = 0;
            }

            uiImage.sprite = currentClip[currentFrameIndex];
        }
    }

    //

    public void OnPerfectHit()
    {
        ShowTalkingState();
        string randomLine = hitLines[Random.Range(0, hitLines.Length)];
        UpdateDialogText(randomLine);
    }

    public void OnMiss()
    {
        ShowTalkingState();
        string randomLine = missLines[Random.Range(0, missLines.Length)];
        UpdateDialogText(randomLine);
    }

    //

    void ShowTalkingState()
    {
        if (!isTalking)
        {
            isTalking = true;
            currentFrameIndex = 0; 
            animTimer = 0f;
        }
        resetTimer = displayDuration;
        if (dialogPanel != null) dialogPanel.SetActive(true);
    }

    void ReturnToIdle()
    {
        isTalking = false;
        currentFrameIndex = 0; 
        animTimer = 0f;
        if (dialogPanel != null) dialogPanel.SetActive(false);
    }

    void UpdateDialogText(string text)
    {
        if (dialogText != null)
        {
            dialogText.text = text;
        }
    }
}
