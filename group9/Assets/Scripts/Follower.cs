using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using FMODUnity;

public class Follower : MonoBehaviour
{
    public string song1 = "00000000>0000011100011100;1010010100100010;1110010101010100;0011011101100010;0000100101010010;1010011100101000;0001000110110001;0111100011110001!";

    public int widthInBeats = 8;
    public int heightInBeats = 4;
	public float widthDuration = 0f;
	public float nextBeat = 0.5f;
	public string nextBeatChar;
	public bool isBeatActive = false;
	public bool isNextBeatActive = false;
	bool stickerIsDone = false;

	public bool isMoving = false;
	public Vector3 startPosition;
	public Vector3 endPosition;
	bool directionRight = true;

	public float points = 0f;
	public float hitMargin = 0.2f;
	// Can't be more than a quarter of a beat duration (if a beat is 1 second, a hitMissMargin < 0.25s)
	public float hitMissMargin = 0.249f;

	public Conductor conductor;
	public TextMeshPro hitLabel;
	public TextMeshPro msLabel;
	public TextMeshPro nowLabel;
	public TextMeshPro playerNowLabel;
	public Timer hitLabelTimer;
	public Timer nowLabelTimer;
	public Timer playerNowLabelTimer;

	InputAction hitAction;

	[Header("--- Audio ---")]
	[SerializeField] EventReference hitEvent;

    //
    [Header("--- Visual Effects & fairy ---")]
    public Fairy fairy;
    public GameObject hitParticlePrefab; 
    public GameObject missParticlePrefab;
    //

	void PlayHitSound()
    {
        RuntimeManager.PlayOneShot(hitEvent);
    }

    void Start()
    {
		// hitLabel = GameObject.Find("HitLabel").GetComponent<TextMeshPro>();
		// msLabel = GameObject.Find("MsLabel").GetComponent<TextMeshPro>();
		// nowLabel = GameObject.Find("NowLabel").GetComponent<TextMeshPro>();
		// hitLabelTimer = GameObject.Find("HitLabelTimer").GetComponent<Timer>();
		// nowLabelTimer = GameObject.Find("NowLabelTimer").GetComponent<Timer>();
		// hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();

		hitAction = InputSystem.actions.FindAction("Hit");

		hitLabel.enabled = false;
		msLabel.enabled = false;
		nowLabel.enabled = false;
		playerNowLabel.enabled = false;
		hitLabelTimer.onTimerComplete = () => { hitLabel.enabled = false; msLabel.enabled = false; };
		nowLabelTimer.onTimerComplete = () => { nowLabel.enabled = false; };
		playerNowLabelTimer.onTimerComplete = () => { playerNowLabel.enabled = false; };

		widthDuration = (float)widthInBeats * conductor.beatDuration;
		nextBeatChar = GetNextSongCharacter();
		nextBeat = 0.5f;
		NextSongCharacter();
		isBeatActive = int.Parse(nextBeatChar) == 1 ? true : false;
		isNextBeatActive = int.Parse(GetNextSongCharacter()) == 1 ? true : false;
		// isNextBeatActive = isBeatActive;
		startPosition = transform.position;
		endPosition = transform.position;
		Move();
	}

	void NextSongCharacter()
	{
		nextBeatChar = song1.Substring(0, 1);
		song1 = song1.Substring(1, song1.Length - 1);
	}

	string GetNextSongCharacter()
	{
		string c = song1.Substring(0, 1);
		if (c == "!")
        {
            return "0";
        }
		else if (c == "!" || c == ">" || c == ";")
		{
			c = song1.Substring(1, 1);
		}
		return c;
	}

    void Update()
	{
		if (stickerIsDone) return;

		if (conductor.currentPosition < 0f) return;

		if (conductor.currentPositionInBeats >= nextBeat)
		{
			NextSongCharacter();
			if (nextBeatChar == "!")
			{
				FinishSticker();
				return;
			}
			if (nextBeatChar == ">")
			{
				isMoving = true;
				NextSongCharacter();
			}
			if (nextBeatChar == ";")
			{
				directionRight = !directionRight;
				transform.position += Vector3.back * 1f;
				startPosition += Vector3.back * 1f;
				endPosition += Vector3.back * 1f;
				NextSongCharacter();
			}
			isBeatActive = int.Parse(nextBeatChar) == 1 ? true : false;
			isNextBeatActive = int.Parse(GetNextSongCharacter()) == 1 ? true : false;
			nextBeat += 0.5f;
			Move();
			// PlayHitSound();

			if (isBeatActive)
            {
                nowLabel.enabled = true;
				nowLabelTimer.StartTimer();
            }
		}

		if (!isMoving) return;
		transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Pow(conductor.currentPositionInBeats % 0.5f / 0.5f, 3f));

		if (hitAction.WasPressedThisDynamicUpdate())
		{
			PlayHitSound();
			playerNowLabel.enabled = true;
			playerNowLabelTimer.StartTimer();

			float timeSinceLastBeat = conductor.currentPositionInBeats % 0.5f;
			float timeToNextBeat = 0.5f - timeSinceLastBeat;

			if (timeSinceLastBeat <= hitMissMargin && isBeatActive)
			{
				if (timeSinceLastBeat <= hitMargin)
                {
					HitSuccesful(timeSinceLastBeat);
				}
				else
                {
                    Missed(timeSinceLastBeat);
                }
				isBeatActive = false;
			}
			else if (timeToNextBeat <= hitMissMargin && isNextBeatActive)
			{
				if (timeToNextBeat <= hitMargin)
                {
					HitSuccesful(-timeToNextBeat);
                }
				else
                {
                    Missed(-timeToNextBeat);
                }
				isNextBeatActive = false;
			}
		}
	}

	void HitSuccesful(float msOff)
    {
		points += 1f;
		msLabel.text = msOff >= 0 ? (msOff * 1000f).ToString("F0") + " ms late" : (-msOff * 1000f).ToString("F0") + " ms early";
		hitLabel.text = "Hit!";
		msLabel.enabled = true;
		hitLabel.enabled = true;
		hitLabelTimer.StartTimer();
		//
        if (fairy != null)
        {
            fairy.OnPerfectHit();
        }

        if (hitParticlePrefab != null)
        {
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        }
    }

	void Missed(float msOff)
	{
		msLabel.text = msOff >= 0 ? (msOff * 1000f).ToString("F0") + " ms late" : (-msOff * 1000f).ToString("F0") + " ms early";
		hitLabel.text = "Miss!";
		msLabel.enabled = true;
		hitLabel.enabled = true;
		hitLabelTimer.StartTimer();
        //
        if (fairy != null)
        {
            fairy.OnMiss();
        }

        if (missParticlePrefab != null)
        {
            Instantiate(missParticlePrefab, transform.position, Quaternion.identity);
        }
    }

	void Move()
	{
		if (!isMoving) return;
		endPosition.x = Mathf.Round(endPosition.x);
		transform.position = endPosition;
		startPosition = endPosition;
		if (directionRight)
		{
			endPosition = startPosition + Vector3.left * 1f;
		}
		else
		{
			endPosition = startPosition + Vector3.right * 1f;
		}
	}

	void FinishSticker()
    {
		endPosition.x = Mathf.Round(endPosition.x);
		transform.position = endPosition;
		isMoving = false;
        stickerIsDone = true;
    }
}
