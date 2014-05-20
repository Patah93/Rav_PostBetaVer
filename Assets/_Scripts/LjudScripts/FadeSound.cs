using UnityEngine;
using System.Collections;

public class FadeSound : MonoBehaviour {

	public bool _activated;

	[Range(0f,1f)]
	public float _fadeSpeed;
	
	AudioSource _audio;

	void Start(){

		_audio = GetComponent<AudioSource>();

	}

	// Update is called once per frame
	void Update () {
	
		if(_activated)
			_audio.volume = Mathf.Lerp(_audio.volume, 1, Time.deltaTime * _fadeSpeed);
		else 
			_audio.volume = Mathf.Lerp(_audio.volume, 0, Time.deltaTime * _fadeSpeed);
	
	}

	public void ChangeState(bool b){
		_activated = b;
	}

	public bool GetBool(){
		return _activated;
	} 
}
