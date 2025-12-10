using UnityEngine;

public class Conductor : MonoBehaviour
{
	public float bpm;
	public float offset;
	public float beatDuration;
	public float currentPosition;
	public float currentPositionInBeats;
	public float musicStartedTime;

	public AudioSource musicSource;

	void Start()
	{
		musicSource = GetComponent<AudioSource>();
		beatDuration = 60f / bpm;
		//musicStartedTime = (float)AudioSettings.dspTime;

		//musicSource.Play();
	}

	void Update()
	{
		currentPosition = (float)(AudioSettings.dspTime - musicStartedTime - offset);
		currentPositionInBeats = currentPosition / beatDuration;
	}

    public void PlaySong()
    {
        musicStartedTime = (float)AudioSettings.dspTime; 
        musicSource.Play();
    }
}
