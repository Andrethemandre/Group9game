using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference hitEvent;
    [SerializeField] EventReference songEvent;
    [SerializeField] GameObject follower;

    public void PlayHitSound()
    {
        RuntimeManager.PlayOneShot(hitEvent);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
