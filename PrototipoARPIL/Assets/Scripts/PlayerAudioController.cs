using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour {

	public GameObject SoundEffectPrototype;
	public AudioClip OilAudio;
	public AudioClip GasAudio;
	public AudioClip MotorAudio;
	public AudioClip MotorNoGasAudio;

	AudioSource _audioSource;
	Transform _player;

	// Use this for initialization
	void Start () {
		_audioSource = GetComponent<AudioSource> ();
		_audioSource.clip = MotorAudio;
		_audioSource.Play ();

		GameObject playerObject = GameObject.FindGameObjectWithTag ("Player");
		_player = playerObject.transform;

		PlayerTruck playerTruck = playerObject.GetComponent<PlayerTruck> ();
		playerTruck.OnGasRefill += PlayGasAudio;
		playerTruck.OnGasRefill += PlayMotorAudio;
		playerTruck.OnOilSlide += PlayOilAudio;

		playerObject.GetComponent<GasController> ().OnNoGas += PlayMotorNoGasAudio;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = _player.position;
	}

	void PlayMotorAudio(object source, System.EventArgs args) {
		PlayAudio (MotorAudio);
	}

	void PlayMotorNoGasAudio(object source, System.EventArgs args) {
		PlayAudio (MotorNoGasAudio);
	}

	void PlayGasAudio(object source, System.EventArgs args) {
		InstantiateAudio (GasAudio);
	}

	void PlayOilAudio(object source, System.EventArgs args) {
		InstantiateAudio (OilAudio);
	}

	void PlayAudio(AudioClip clip) {
		if (_audioSource.clip != clip) {
			_audioSource.clip = clip;
			_audioSource.Play ();
		}
	}

	void InstantiateAudio(AudioClip clip) {
		Instantiate (SoundEffectPrototype).GetComponent<SoundEffectController> ().Play (clip);
	}
}
