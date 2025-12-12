using UnityEngine;
using FMODUnity;
using FMOD;
using FMOD.Studio;
using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;

public class Conductor : MonoBehaviour
{
	public float bpm;
	public int songOffsetMs;
	float songOffset;
	public int globalOffsetMs;
	float globalOffset;
	public double beatDuration;
	public double currentPosition;
	public double currentPositionInBeats;
	public double musicStartedTime;
	public float volumeDb = -6f;

	[SerializeField] EventReference song1Event;
	EventInstance song1Instance;
	ChannelGroup channelGroup;

	int sampleRate = 48000;

	void Awake()
	{
		beatDuration = 60f / bpm;

		songOffset = (float)songOffsetMs / 1000f;
		globalOffset = (float)globalOffsetMs / 1000f;

		UnityEngine.Debug.Log("Create song instance");
		song1Instance = RuntimeManager.CreateInstance(song1Event);
		song1Instance.start();
		song1Instance.setPaused(true);
		song1Instance.getChannelGroup(out channelGroup);

		sampleRate = 48000;
		UnityEngine.Debug.Log("Sample Rate Conductor: " + sampleRate);
	}

	float getSongPositionDSPClock()
	{
		if (!song1Instance.isValid())
		{
			return 0f;
		}
		song1Instance.getChannelGroup(out channelGroup);
		channelGroup.getDSPClock(
			out ulong dspClock, out ulong parentClock
		);
		// UnityEngine.Debug.Log("DSP Clock: " + dspClock + " / DSP Time: " + ((float)dspClock / (float)sampleRate));
		return (float)dspClock / (float)sampleRate;
	}

	float getSongPosition()
	{
		if (!song1Instance.isValid())
		{
			return 0f;
		}
		song1Instance.getTimelinePosition(out int timelinePosition);
		return (float)timelinePosition / 1000f;
	}

	void Update()
	{
		currentPosition = getSongPositionDSPClock() - musicStartedTime - songOffset - globalOffset;
		// currentPosition = getSongPosition() - musicStartedTime - songOffset - globalOffset;
		currentPositionInBeats = currentPosition / beatDuration;
	}

    public void PlaySong()
    {
		song1Instance.setPaused(false);
		song1Instance.setVolume(DbToLinear(volumeDb));

        musicStartedTime = getSongPositionDSPClock();
        // musicStartedTime = getSongPosition();
		UnityEngine.Debug.Log("Music Started Time: " + musicStartedTime);
    }

	float DbToLinear(float db)
	{
		return Mathf.Pow(10.0f, db / 10.0f);
	}
}
