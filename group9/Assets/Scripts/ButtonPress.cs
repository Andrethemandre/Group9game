using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPress : MonoBehaviour
{
    public string actionName = "Hit";
    private InputAction hitAction;
    public float moveAmount = 0.6f;
    Vector3 originalPosition;

    void Start()
    {
        hitAction = InputSystem.actions.FindAction(actionName);
        originalPosition = transform.position;
    }

    void Update()
    {
        Vector3 targetPosition = originalPosition;
        if (hitAction.IsPressed())
        {
            targetPosition += Vector3.down * moveAmount;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 10f);
    }
}
