using FMOD;
using UnityEngine;
using UnityEngine.UI;

public class DebugHitBar : MonoBehaviour
{
    public Timer fadeTimer;
    public RawImage image;
    public Color color = Color.white;
    public float time = 0f;

    void Start()
    {
        // color = image.color;
        fadeTimer.onTimerComplete += () =>
        {
            Destroy(gameObject);
        };
    }

    void Update()
    {
        if (!fadeTimer.playing) return;

        image.color = new Color(
            color.r, color.g,
            (-fadeTimer.timeLeft / fadeTimer.waitTime + 1f) * 0.6f, 
            fadeTimer.timeLeft / fadeTimer.waitTime
        );
    }
}
