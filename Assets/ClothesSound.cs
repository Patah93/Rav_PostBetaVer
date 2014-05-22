using UnityEngine;
using System.Collections;

public class ClothesSound : MonoBehaviour {

	Animator _ani;

	AudioSource _audio;

	public AudioClip _walkSound;

	public AudioClip _runSound;

	/*
	[Range(0.0f, 1.0f)]
	public float _minVolume = 0.0f;
	[Range(0.0f, 1.0f)]
	public float _maxVolume = 1.0f;
	*/


	// Use this for initialization
	void Start () {
		_ani.GetComponent<Animator>();
		_audio =GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo aniState = _ani.GetCurrentAnimatorStateInfo(0);
		if(aniState.IsName("Run") || aniState.IsName("Jump")  || aniState.IsName("Fallin'")  || aniState.IsName("Jump")  || aniState.IsName("Idle Jump")  || aniState.IsName("Jump")){
			if(_audio.clip != _runSound){
				/* TODO fade into runsound */
				_audio.clip = _runSound;
				_audio.Play();
			}
		}else if(aniState.IsName("Push") || aniState.IsName("Pull")){
			if(_audio.clip != _walkSound){
				/* TODO fade into walksound */
				_audio.clip = _walkSound;
				_audio.Play();
			}
		}else{
			/* TODO fade out sound completely */
			_audio.Stop();
		}
	}
}
