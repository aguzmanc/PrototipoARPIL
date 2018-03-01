using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public AudioClip[] AmbientalSounds;
	public int MaximumSourcesAtATime = 3;
	public float MinSoundInterval = 5f;
	public float SoundIntervalDifference = 3f;

	int _numberOfSoundsPlaying = 0;
	List<AudioSource> _audioSources = new List<AudioSource> ();

	// Use this for initialization
	void Awake () {
		for (int i = 0; i < MaximumSourcesAtATime; i++) {
			AudioSource newSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			newSource.playOnAwake = false;
			_audioSources.Add (newSource);
		}
	}

	void Start() {
		for (int i = 0; i < _audioSources.Count; i++) {
			float timeInterval = Random.Range (MinSoundInterval, MinSoundInterval + SoundIntervalDifference);
			StartCoroutine (SourcePlayOnInterval(_audioSources[i], timeInterval));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PlayIfPossible(AudioSource source) {
		if (source.isPlaying)
			return;
		
		if (_numberOfSoundsPlaying >= MaximumSourcesAtATime)
			return;
		
		if (AmbientalSounds.Length == 0)
			return;

		AudioClip clip = AmbientalSounds[Random.Range (0, AmbientalSounds.Length)];
		source.clip = clip;
		source.Play ();
		_numberOfSoundsPlaying++;
		StartCoroutine (StopSourceAfter (source, clip.length));
	}

	IEnumerator SourcePlayOnInterval(AudioSource source, float secondsToWait) {
		while (true) {
			yield return new WaitForSeconds (secondsToWait);
			PlayIfPossible (source);
		}
	}

	IEnumerator StopSourceAfter(AudioSource source, float secondsToStop) {
		yield return new WaitForSeconds (secondsToStop);
		source.Stop ();
		_numberOfSoundsPlaying--;
	}
}
