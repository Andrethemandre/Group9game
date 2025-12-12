using UnityEngine;

public class Sticker : MonoBehaviour
{
    public GameObject sticker;
    public Conductor conductor;
    Vector3 newPosition;

    void Start()
    {
        newPosition = sticker.transform.position;
    }

    void Update()
    {
        sticker.transform.position = Vector3.Lerp(sticker.transform.position, newPosition, Time.deltaTime * 10);
    }

    public void MoveSticker(Vector3 toPosition)
    {
        newPosition = toPosition;
    }
}
