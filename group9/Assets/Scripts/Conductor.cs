using UnityEngine;
using FMODUnity;
using FMOD;
using FMOD.Studio;
using System;
using System.Collections;

public class Conductor : MonoBehaviour
{
	public float bpm;
	public float songOffset;
	public float globalOffset;
	public float beatDuration;
	public float currentPosition;
	public float currentPositionInBeats;
	public float musicStartedTime;
	public float volumeDb = -6f;

	[SerializeField] EventReference song1Event;
	EventInstance song1Instance;
	// ChannelGroup channelGroup;

	void Start()
	{
		beatDuration = 60f / bpm;

		song1Instance = RuntimeManager.CreateInstance(song1Event);
		song1Instance.start();
		// song1Instance.getChannelGroup(out channelGroup);
	}

	void Update()
	{
		song1Instance.getTimelinePosition(out int timelinePosition);
		// song1Instance.getChannelGroup(out channelGroup);
		// channelGroup.getDSPClock(
		// 	out ulong dspClock, out ulong parentClock
		// );
		// float dspTime = (float)dspClock / (float)AudioSettings.outputSampleRate;
		// UnityEngine.Debug.Log("DSP Time: " + ((float)dspClock / (float)AudioSettings.outputSampleRate) + " / " + "Get position time: " + ((float)timelinePosition / 1000f));
		currentPosition = ((float)timelinePosition / 1000f) - musicStartedTime - songOffset - globalOffset;
		// currentPosition = dspTime - musicStartedTime - songOffset - globalOffset;
		currentPositionInBeats = currentPosition / beatDuration;
	}

    public void PlaySong()
    {
		song1Instance = RuntimeManager.CreateInstance(song1Event);
		song1Instance.start();

		// song1Instance.getChannelGroup(out channelGroup);
		song1Instance.getTimelinePosition(out int timelinePosition);
		musicStartedTime = (float)timelinePosition / 1000f;

		song1Instance.setVolume(DbToLinear(volumeDb));

		// channelGroup.getDSPClock(
		// 	out ulong dspClock, out ulong parentClock
		// );
		// float dspTime = (float)dspClock / AudioSettings.outputSampleRate;
		// UnityEngine.Debug.Log("DSP Time: " + dspTime + " / " + "Get position time: " + ((float)timelinePosition / 1000f));
    }

	float DbToLinear(float db)
	{
		return Mathf.Pow(10.0f, db / 10.0f);
	}
}
