using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    [Header("--- Targets---")]
    public Transform introTarget;   
    public Transform gameplayTarget;

    [Header("--- Settings ---")]
    public float transitionDuration = 2.0f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); 

    private bool isMoving = false;

    void Start()
    {
        if (introTarget != null)
        {
            transform.position = introTarget.position;
            transform.rotation = introTarget.rotation;
        }
    }


    /// <summary>
    /// </summary>
    /// <param name="onComplete"></param>
    public void MoveToGameplayView(Action onComplete = null)
    {
        if (!isMoving && gameplayTarget != null)
        {
            StartCoroutine(TransitionRoutine(gameplayTarget, onComplete));
        }
    }

    /// <summary>
    /// </summary>
    public void MoveToIntroView(Action onComplete = null)
    {
        if (!isMoving && introTarget != null)
        {
            StartCoroutine(TransitionRoutine(introTarget, onComplete));
        }
    }

    IEnumerator TransitionRoutine(Transform target, Action onComplete)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;


        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float percentage = timer / transitionDuration;

            float curveValue = moveCurve.Evaluate(percentage);

            transform.position = Vector3.Lerp(startPos, target.position, curveValue);
            transform.rotation = Quaternion.Lerp(startRot, target.rotation, curveValue);

            yield return null; 
        }

        transform.position = target.position;
        transform.rotation = target.rotation;

        isMoving = false;

        onComplete?.Invoke();
    }
}
