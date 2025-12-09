using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public string song1 = "00000000>0011100011100000;0100010010100101;1110010101010100;0011011101100010;0000100101010010;1010011100101000;0001000110110001;0111100011110001!";

    public int widthInBeats = 8;
    public int heightInBeats = 4;
	public float widthDuration = 0f;
	public float nextBeat = 0.5f;
	public string nextBeatChar;
	public bool isBeatActive = false;
	bool stickerIsDone = false;

	public bool isMoving = false;
	public Vector3 startPosition;
	public Vector3 endPosition;
	bool directionRight = true;

	public float points = 0f;
	public float hitMargin = 0.2f;

	public Conductor conductor;

    void Start()
    {
		widthDuration = (float)widthInBeats * conductor.beatDuration;
		// nextBeatChar = "0";
		nextBeatChar = song1.Substring(0, 1);
		song1 = song1.Substring(1, song1.Length - 1);
		isBeatActive = int.Parse(nextBeatChar) == 1 ? true : false;
		startPosition = transform.position;
		endPosition = transform.position;
		Move();
	}

	void NextSongCharacter()
	{
		nextBeatChar = song1.Substring(0, 1);
		song1 = song1.Substring(1, song1.Length - 1);
	}

    void Update()
	{
		if (stickerIsDone) return;

		if (conductor.currentPositionInBeats >= nextBeat)
		{
			NextSongCharacter();
			if (nextBeatChar == "!")
			{
				isMoving = false;
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
			nextBeat += 0.5f;
			Move();
		}

		if (!isMoving) return;
		transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Pow(conductor.currentPositionInBeats % 0.5f / 0.5f, 3f));

		if (Input.GetKey(KeyCode.Space))
		{
			if (isBeatActive)
            {
                points += 1f;
				Debug.Log("Hit! Points: " + points);
            }
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
        stickerIsDone = true;
    }
}
