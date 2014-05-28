using UnityEngine;
using System.Collections;

public class FadeSound : MonoBehaviour {

	public bool _activated;

	[Range(0f,1f)]
	public float _fadeSpeed, _maxVolume, _minVolume;
	
	AudioSource _audio;

	void Start(){

		_audio = GetComponent<AudioSource>();

	}

	// Update is called once per frame
	void Update () {
	
		if(_activated)
			_audio.volume = Mathf.Lerp(_audio.volume, _maxVolume, Time.deltaTime * _fadeSpeed);
		else 
			_audio.volume = Mathf.Lerp(_audio.volume, _minVolume, Time.deltaTime * _fadeSpeed);
	
	}

	public void ChangeState(bool b){
		_activated = b;
	}

	public bool GetBool(){
		return _activated;
	} 

	public void SetMinFade(float f){
		_minVolume = f;
	}

	public void SetMaxFade(float f){
		_maxVolume = f;
	}
}
